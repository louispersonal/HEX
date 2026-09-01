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
        foreach (Hex data in world.Grid.GetValidHexes())
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

    private static void FillRegion(WorldData world, Hex startHex, Region newRegion, int maxRegionSize)
    {
        Biome targetBiome = startHex.ExtraData.Biome;
        newRegion.Biome = targetBiome;

        Stack<Hex> stack = new Stack<Hex>();
        startHex.ExtraData.SetRegionID(newRegion.ID);
        stack.Push(startHex);

        while (stack.Count > 0 && newRegion.Size < maxRegionSize)
        {
            Hex hex = stack.Pop();
            newRegion.Size++;
            newRegion.TotalLowVegetation += hex.ExtraData.LowVegetation;
            newRegion.TotalHighVegetation += hex.ExtraData.HighVegetation;

            foreach (AxialCoordinate dir in AxialDirections.Directions)
            {
                Hex neighbor = world.Grid.TryGetHex(hex.Coord + dir, out var outHex)? outHex : null;

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
        ResourceCollection regionSnapshot = region.PreviewAvailableResources();
        ResourceStockpile regionVirtualStockpile = new ResourceStockpile(regionSnapshot, null);
        
        List<ResourceRequest> requests = new List<ResourceRequest>();
        List<SpeciesDefinition> speciesCandidates = new List<SpeciesDefinition>();
        foreach (SpeciesDefinition species in GameController.Instance.StaticDatabases.SpeciesDatabase.Items)
        {
            if (species.Biomes.Contains(region.Biome))
            {
                speciesCandidates.Add(species);
            }
        }

        speciesCandidates.Shuffle();
        int numberOfSpecies = Random.Range(8, 12);
        if (numberOfSpecies < speciesCandidates.Count)
        {
            for (int n = speciesCandidates.Count; n > numberOfSpecies; n--)
            {
                speciesCandidates.RemoveAt(n);
            }
        }
        
        
    }
}
