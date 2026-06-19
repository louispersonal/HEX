using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegionGen
{
    public static Region[] CreateRegions(WorldData world, WorldGenParameters parameters)
    {
        ushort currentRegionId = 1;
        List<Region> regions = new List<Region>();
        int worldSize = world.Grid.Width * world.Grid.Height;
        int maxRegionSize = Mathf.RoundToInt(parameters.MaxRegionSizeRatio * worldSize);
        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (data.ExtraData.RegionId == 0 && !data.ExtraData.IsSea) // this hex does not belong to a region yet
            {
                Region newRegion = new Region(currentRegionId, data.Coord);
                newRegion.Size = 1;
                newRegion.TotalLowVegetation = data.ExtraData.LowVegetation;
                newRegion.TotalHighVegetation = data.ExtraData.HighVegetation;
                FillRegion(world, data, newRegion, maxRegionSize);
                PopulateRegion(world, newRegion);

                // upkeep
                regions.Add(newRegion);
                currentRegionId++;
            }
        }

        return regions.ToArray();
    }

    private static void FillRegion(WorldData world, HexData startHex, Region newRegion, int maxRegionSize)
    {
        Biome targetBiome = startHex.ExtraData.Biome;

        Stack<HexData> stack = new Stack<HexData>();
        startHex.ExtraData.SetRegionID(newRegion.ID);
        stack.Push(startHex);

        while (stack.Count > 0 && newRegion.Size < maxRegionSize)
        {
            HexData hex = stack.Pop();
            newRegion.Size++;
            newRegion.TotalLowVegetation += hex.ExtraData.LowVegetation;
            newRegion.TotalHighVegetation += hex.ExtraData.HighVegetation;

            foreach (AxialCoordinate dir in AxialDirections.Directions)
            {
                HexData neighbor = world.Grid.TryGetHex(hex.Coord + dir, out var outHex)? outHex : null;

                if (neighbor == null) continue;
                if (neighbor.ExtraData.RegionId != 0) continue;
                if (neighbor.ExtraData.Biome != targetBiome) continue;

                neighbor.ExtraData.SetRegionID(newRegion.ID);
                stack.Push(neighbor);
            }
        }
    }

    private static void PopulateRegion(WorldData world, Region newRegion)
    {
        float lowVeg = newRegion.GetLowVegetationNutrition();
        float highVeg = newRegion.GetHighVegetationNutrition();
        float fish = newRegion.GetRiverNutrition();
        float microfauna = newRegion.GetMicroFaunaNutrition();

        Biome biome = world.Grid.TryGetHex(newRegion.SeedCoord, out var outHex)
            ? outHex.ExtraData.Biome
            : Biome.Sea;

        Dictionary<ushort, SpeciesProfile> speciesData = world.AnimalSpeciesByBiome[biome];
        Dictionary<ushort, ArchetypeProfile> archetypes = world.AnimalArchetypes;

        // Food pools available to primary consumers.
        Dictionary<FoodType, float> foodPools = new()
    {
        { FoodType.LowVegetation, lowVeg },
        { FoodType.HighVegetation, highVeg },
        { FoodType.Microfauna, microfauna },
        { FoodType.Fish, fish },
        { FoodType.Meat, 0f },
        { FoodType.Carrion, 0f }
    };

        List<AnimalGroup> generatedGroups = new();

        // Group species by archetype.
        Dictionary<ushort, List<SpeciesProfile>> speciesByArchetype = new();

        foreach (SpeciesProfile species in speciesData.Values)
        {
            if (!speciesByArchetype.ContainsKey(species.ArchetypeId))
                speciesByArchetype[species.ArchetypeId] = new List<SpeciesProfile>();

            speciesByArchetype[species.ArchetypeId].Add(species);
        }

        // Populate non-meat eaters first.
        float herbivoreBiomass = 0f;

        foreach (var pair in speciesByArchetype)
        {
            ushort archetypeId = pair.Key;

            if (!archetypes.TryGetValue(archetypeId, out ArchetypeProfile archetype))
                continue;

            if (archetype.Eats.Contains(FoodType.Meat) || archetype.Eats.Contains(FoodType.Carrion))
                continue;

            float availableNutrition = GetAvailableNutrition(archetype, foodPools);

            if (availableNutrition <= 0f)
                continue;

            // Split available nutrition between all archetypes/species later if needed.
            // For now this archetype consumes from its valid pools.
            float allocatedNutrition = availableNutrition * 0.25f;

            SpendNutrition(archetype, foodPools, allocatedNutrition);

            int count = Mathf.FloorToInt(allocatedNutrition / archetype.NutritionRequirement);

            if (count <= 0)
                continue;

            SpeciesProfile chosenSpecies = PickSpecies(pair.Value, newRegion.ID, archetypeId);

            generatedGroups.Add(new AnimalGroup
            {
                SpeciesId = chosenSpecies.SpeciesId,
                RegionId = newRegion.ID,
                Count = count
            });

            herbivoreBiomass += count * archetype.NutritionRequirement;
        }

        // Convert some herbivore biomass into predator/scavenger food.
        foodPools[FoodType.Meat] = herbivoreBiomass * 0.10f;
        foodPools[FoodType.Carrion] = herbivoreBiomass * 0.03f;

        // Populate meat/carrion eaters.
        foreach (var pair in speciesByArchetype)
        {
            ushort archetypeId = pair.Key;

            if (!archetypes.TryGetValue(archetypeId, out ArchetypeProfile archetype))
                continue;

            bool eatsMeatOrCarrion =
                archetype.Eats.Contains(FoodType.Meat) ||
                archetype.Eats.Contains(FoodType.Carrion);

            if (!eatsMeatOrCarrion)
                continue;

            float availableNutrition = GetAvailableNutrition(archetype, foodPools);

            if (availableNutrition <= 0f)
                continue;

            float allocatedNutrition = availableNutrition * 0.35f;

            SpendNutrition(archetype, foodPools, allocatedNutrition);

            int count = Mathf.FloorToInt(allocatedNutrition / archetype.NutritionRequirement);

            if (count <= 0)
                continue;

            SpeciesProfile chosenSpecies = PickSpecies(pair.Value, newRegion.ID, archetypeId);

            generatedGroups.Add(new AnimalGroup
            {
                SpeciesId = chosenSpecies.SpeciesId,
                RegionId = newRegion.ID,
                Count = count
            });
        }

        newRegion.AnimalsInRegion = generatedGroups;
    }

    private static float GetAvailableNutrition(
        ArchetypeProfile archetype,
        Dictionary<FoodType, float> foodPools)
    {
        float total = 0f;

        foreach (FoodType food in archetype.Eats)
        {
            if (foodPools.TryGetValue(food, out float amount))
                total += amount;
        }

        return total;
    }

    private static void SpendNutrition(
        ArchetypeProfile archetype,
        Dictionary<FoodType, float> foodPools,
        float amountToSpend)
    {
        float available = GetAvailableNutrition(archetype, foodPools);

        if (available <= 0f)
            return;

        foreach (FoodType food in archetype.Eats)
        {
            if (!foodPools.ContainsKey(food))
                continue;

            float share = foodPools[food] / available;
            float spent = amountToSpend * share;

            foodPools[food] = Mathf.Max(0f, foodPools[food] - spent);
        }
    }

    private static SpeciesProfile PickSpecies(
        List<SpeciesProfile> species,
        ushort regionId,
        ushort archetypeId)
    {
        // Deterministic pseudo-random choice.
        int index = Mathf.Abs((regionId * 397) ^ archetypeId) % species.Count;
        return species[index];
    }
}
