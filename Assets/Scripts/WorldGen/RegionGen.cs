using System.Collections;
using System.Collections.Generic;
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

    private static void PopulateRegion(WorldData world, Region newRegion)
    {
        float totalLowVegetationNutrition = newRegion.GetLowVegetationNutrition();
        float totalHighVegetationNutrition = newRegion.GetHighVegetationNutrition();
        float totalRiverNutrition = newRegion.GetRiverNutrition();
        float totalMicroFaunaNutrition = newRegion.GetMicroFaunaNutrition();


    }
}
