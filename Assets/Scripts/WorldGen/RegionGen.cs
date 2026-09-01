using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegionGen
{
    private const int MinSpeciesPerRegion = 8;
    private const int MaxSpeciesPerRegion = 12;

    // This is deliberately a world-generation abstraction rather than a literal
    // ecological conversion. Tune it until predator populations look reasonable.
    private const float PredatorFoodFraction = 0.01f;
    private const float MinPopulationVariance = 0.85f;
    private const float MaxPopulationVariance = 1.15f;

    public static Region[] CreateRegions(WorldData world, WorldGenParameters parameters)
    {
        ushort currentRegionId = 1;
        List<Region> regions = new();

        int worldSize = world.Grid.Width * world.Grid.Height;
        int maxRegionSize = Mathf.Max(
            1,
            Mathf.RoundToInt(parameters.MaxRegionSizeRatio * worldSize));

        foreach (Hex hex in world.Grid.GetValidHexes())
        {
            if (hex.ExtraData.RegionId != 0 || hex.ExtraData.IsSea)
                continue;

            Region newRegion = new(currentRegionId, hex.Coord);
            FillRegion(world, hex, newRegion, maxRegionSize);

            regions.Add(newRegion);
            currentRegionId++;
        }

        return regions.ToArray();
    }

    private static void FillRegion(
        WorldData world,
        Hex startHex,
        Region newRegion,
        int maxRegionSize)
    {
        Biome targetBiome = startHex.ExtraData.Biome;
        newRegion.Biome = targetBiome;

        Stack<Hex> stack = new();
        startHex.ExtraData.SetRegionID(newRegion.ID);
        stack.Push(startHex);

        while (stack.Count > 0)
        {
            Hex hex = stack.Pop();

            newRegion.Size++;
            newRegion.TotalLowVegetation += hex.ExtraData.LowVegetation;
            newRegion.TotalHighVegetation += hex.ExtraData.HighVegetation;

            foreach (AxialCoordinate direction in AxialDirections.Directions)
            {
                // Every hex already in the stack has been assigned this ID. Do not
                // claim more hexes than this region can actually process.
                if (newRegion.Size + stack.Count >= maxRegionSize)
                    break;

                if (!world.Grid.TryGetHex(hex.Coord + direction, out Hex neighbor))
                    continue;

                if (neighbor.ExtraData.RegionId != 0)
                    continue;

                if (neighbor.ExtraData.IsSea ||
                    neighbor.ExtraData.Biome != targetBiome)
                {
                    continue;
                }

                neighbor.ExtraData.SetRegionID(newRegion.ID);
                stack.Push(neighbor);
            }
        }
    }

    public static void PopulateRegion(
        WorldData world,
        StaticDatabases databases,
        Region region)
    {
        region.Animals.Clear();

        List<SpeciesDefinition> selectedSpecies = SelectSpecies(
            databases,
            region.Biome);

        if (selectedSpecies.Count == 0)
            return;

        List<SpeciesDefinition> primaryConsumers = selectedSpecies
            .Where(species => HasNonMeatFood(species, databases))
            .ToList();

        List<SpeciesDefinition> meatOnlyPredators = selectedSpecies
            .Where(species => IsMeatOnly(species, databases))
            .ToList();

        ResourceCollection regionalFood = region.PreviewAvailableResources(world.Grid);

        PopulatePrimaryConsumers(
            region,
            primaryConsumers,
            regionalFood,
            databases);

        PopulatePredators(
            region,
            meatOnlyPredators,
            databases);
    }

    private static List<SpeciesDefinition> SelectSpecies(
        StaticDatabases databases,
        Biome biome)
    {
        List<SpeciesDefinition> candidates = databases.SpeciesDatabase.Items
            .Where(species => species.Biomes.Contains(biome))
            .ToList();

        Shuffle(candidates);

        int desiredCount = Random.Range(
            MinSpeciesPerRegion,
            MaxSpeciesPerRegion + 1);

        if (candidates.Count > desiredCount)
        {
            candidates.RemoveRange(
                desiredCount,
                candidates.Count - desiredCount);
        }

        return candidates;
    }

    private static void PopulatePrimaryConsumers(
        Region region,
        List<SpeciesDefinition> species,
        ResourceCollection regionalFood,
        StaticDatabases databases)
    {
        Dictionary<SpeciesID, float> allocations = AllocatePlantFood(
            regionalFood,
            species,
            databases);

        foreach (SpeciesDefinition candidate in species)
        {
            AnimalArchetypeDefinition archetype =
                databases.AnimalArchetypeDatabase.Get(
                    candidate.ArchetypeId);

            if (archetype.NutritionRequired <= 0f)
                continue;

            float allocatedFood = allocations.GetValueOrDefault(
                candidate.Id,
                0f);

            int population = Mathf.FloorToInt(
                allocatedFood / archetype.NutritionRequired *
                Random.Range(
                    MinPopulationVariance,
                    MaxPopulationVariance));

            if (population > 0)
                region.Animals[candidate.Id] = population;
        }
    }

    private static Dictionary<SpeciesID, float> AllocatePlantFood(
        ResourceCollection regionalFood,
        List<SpeciesDefinition> species,
        StaticDatabases databases)
    {
        Dictionary<SpeciesID, float> allocations = new();

        foreach (SpeciesDefinition candidate in species)
            allocations[candidate.Id] = 0f;

        foreach (ResourceID resource in regionalFood.GetAllResourceIDs())
        {
            if (resource == ResourceIDMap.Meat)
                continue;

            Dictionary<SpeciesID, float> consumerWeights = new();
            float totalWeight = 0f;

            foreach (SpeciesDefinition candidate in species)
            {
                AnimalArchetypeDefinition archetype =
                    databases.AnimalArchetypeDatabase.Get(
                        candidate.ArchetypeId);

                int plantDietCount = archetype.Diet.Count(
                    food => food != ResourceIDMap.Meat);

                if (plantDietCount == 0 ||
                    !archetype.Diet.Contains(resource))
                {
                    continue;
                }

                // Generalists spread their competitive weight across their foods,
                // while stronger foragers claim a larger share of contested food.
                float weight = Mathf.Max(
                    0f,
                    archetype.ForagingAbility) / plantDietCount;

                if (weight <= 0f)
                    continue;

                consumerWeights[candidate.Id] = weight;
                totalWeight += weight;
            }

            if (totalWeight <= 0f)
                continue;

            float available = regionalFood.Get(resource);

            foreach (KeyValuePair<SpeciesID, float> consumer in consumerWeights)
            {
                allocations[consumer.Key] +=
                    available * consumer.Value / totalWeight;
            }
        }

        return allocations;
    }

    private static void PopulatePredators(
        Region region,
        List<SpeciesDefinition> predators,
        StaticDatabases databases)
    {
        if (predators.Count == 0 || region.Animals.Count == 0)
            return;

        int largestPredatorSize = predators.Max(species =>
            (int)databases.AnimalArchetypeDatabase.Get(
                species.ArchetypeId).Size);

        float totalAccessiblePrey = GetAvailablePreyMeat(
            region.Animals,
            databases,
            largestPredatorSize);

        if (totalAccessiblePrey <= 0f)
            return;

        Dictionary<SpeciesID, float> predatorWeights = new();
        float totalWeight = 0f;

        foreach (SpeciesDefinition predator in predators)
        {
            AnimalArchetypeDefinition archetype =
                databases.AnimalArchetypeDatabase.Get(
                    predator.ArchetypeId);

            float accessiblePrey = GetAvailablePreyMeat(
                region.Animals,
                databases,
                archetype.Size);

            float weight = Mathf.Max(0f, archetype.ForagingAbility) *
                           accessiblePrey / totalAccessiblePrey;

            if (weight <= 0f)
                continue;

            predatorWeights[predator.Id] = weight;
            totalWeight += weight;
        }

        if (totalWeight <= 0f)
            return;

        float predatorFoodBudget =
            totalAccessiblePrey * PredatorFoodFraction;

        foreach (SpeciesDefinition predator in predators)
        {
            if (!predatorWeights.TryGetValue(
                    predator.Id,
                    out float weight))
            {
                continue;
            }

            AnimalArchetypeDefinition archetype =
                databases.AnimalArchetypeDatabase.Get(
                    predator.ArchetypeId);

            if (archetype.NutritionRequired <= 0f)
                continue;

            float allocatedFood =
                predatorFoodBudget * weight / totalWeight;

            int population = Mathf.FloorToInt(
                allocatedFood / archetype.NutritionRequired *
                Random.Range(
                    MinPopulationVariance,
                    MaxPopulationVariance));

            if (population > 0)
                region.Animals[predator.Id] = population;
        }
    }

    private static float GetAvailablePreyMeat(
        Dictionary<SpeciesID, int> animals,
        StaticDatabases databases,
        int predatorSize)
    {
        float meatAvailable = 0f;

        foreach (KeyValuePair<SpeciesID, int> animal in animals)
        {
            SpeciesDefinition species =
                databases.SpeciesDatabase.Get(animal.Key);

            AnimalArchetypeDefinition archetype =
                databases.AnimalArchetypeDatabase.Get(
                    species.ArchetypeId);

            if (archetype.Size <= predatorSize)
            {
                meatAvailable +=
                    archetype.NutritionProvided * animal.Value;
            }
        }

        return meatAvailable;
    }

    private static bool HasNonMeatFood(
        SpeciesDefinition species,
        StaticDatabases databases)
    {
        AnimalArchetypeDefinition archetype =
            databases.AnimalArchetypeDatabase.Get(
                species.ArchetypeId);

        return archetype.Diet.Any(
            resource => resource != ResourceIDMap.Meat);
    }

    private static bool IsMeatOnly(
        SpeciesDefinition species,
        StaticDatabases databases)
    {
        AnimalArchetypeDefinition archetype =
            databases.AnimalArchetypeDatabase.Get(
                species.ArchetypeId);

        return archetype.Diet.Length > 0 &&
               archetype.Diet.All(
                   resource => resource == ResourceIDMap.Meat);
    }

    private static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int swapIndex = Random.Range(0, i + 1);
            (list[i], list[swapIndex]) =
                (list[swapIndex], list[i]);
        }
    }
}
