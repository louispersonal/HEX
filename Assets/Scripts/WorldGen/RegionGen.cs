using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public static void PopulateRegion(WorldData world, StaticDatabases databases, Region region)
    {
        if (!world.Grid.TryGetHex(region.SeedCoord, out HexData seedHex)) return;
        Biome regionBiome = seedHex.ExtraData.Biome;
        var biomeVegetationProfile = VegetationProfiles.Profiles[regionBiome];
        
        Dictionary<ResourceID, float> regionResourceYields = new Dictionary<ResourceID, float>();
        
        foreach (ResourceDailyYield yield in biomeVegetationProfile.LowVegetationProfile.DailyYields)
        {
            float quantity = yield.MaximumDailyYield * region.TotalLowVegetation;
            regionResourceYields.TryGetValue(yield.ResourceId, out float existing);
            regionResourceYields[yield.ResourceId] = existing + quantity;
        }
        
        foreach (ResourceDailyYield yield in biomeVegetationProfile.HighVegetationProfile.DailyYields)
        {
            float quantity = yield.MaximumDailyYield * region.TotalHighVegetation;
            regionResourceYields.TryGetValue(yield.ResourceId, out float existing);
            regionResourceYields[yield.ResourceId] = existing + quantity;
        }
        
        // herbivores first
        foreach (SpeciesDefinition species in databases.SpeciesDatabase.Items)
        {
            if (!species.Biomes.Contains(region.Biome)) continue;
            
            AnimalArchetypeDefinition archetype = databases.AnimalArchetypeDatabase.Get(species.ArchetypeId);
            float totalAvailableNutrition = 0f;
            foreach (ResourceID resource in archetype.Diet)
            {
                if (resource != ResourceIDMap.Meat)
                {
                    totalAvailableNutrition += regionResourceYields[resource] * archetype.ForagingAbility;
                }
            }

            float largestPossiblePopulation = totalAvailableNutrition / archetype.NutritionRequired;
            if (largestPossiblePopulation > 0) region.Animals[species.Id] = Mathf.RoundToInt(largestPossiblePopulation);
        }
        
        // then predators
        foreach (SpeciesDefinition species in databases.SpeciesDatabase.Items)
        {
            if (!species.Biomes.Contains(region.Biome)) continue;
            
            AnimalArchetypeDefinition archetype = databases.AnimalArchetypeDatabase.Get(species.ArchetypeId);
            float totalAvailableNutrition = 0f;
            foreach (ResourceID resource in archetype.Diet)
            {
                if (resource == ResourceIDMap.Meat)
                {
                    totalAvailableNutrition += GetAvailablePreyMeat(region.Animals, databases, regionBiome, archetype.Size) * archetype.ForagingAbility;
                }
            }

            float largestPossiblePopulation = totalAvailableNutrition / archetype.NutritionRequired;
            if (largestPossiblePopulation > 0) region.Animals[species.Id] = Mathf.RoundToInt(largestPossiblePopulation);
        }
    }

    private static float GetAvailablePreyMeat(Dictionary<SpeciesID, int> animals, StaticDatabases databases, Biome biome, int predatorSize)
    {
        float meatAvailable = 0f;
        foreach (var animal in animals.Keys)
        {
            AnimalArchetypeDefinition archetype = databases.AnimalArchetypeDatabase.Get
                (databases.SpeciesDatabase.Get(animal).ArchetypeId);
            if (archetype.Size <= predatorSize) meatAvailable += archetype.NutritionProvided * animals[animal];
        }

        return meatAvailable;
    }
}
