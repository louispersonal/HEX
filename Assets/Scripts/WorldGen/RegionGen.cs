using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionGen
{
    public static Region[] CreateRegions(WorldData world)
    {
        ushort currentRegionId = 1;
        List<Region> regions = new List<Region>();
        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (data.ExtraData.RegionId == 0 && !data.ExtraData.IsSea) // this hex does not belong to a region yet
            {
                Region newRegion = new Region(currentRegionId, data.Coord);
                newRegion.Size = 1;
                FillRegion(world, data, newRegion);
                regions.Add(newRegion);
                currentRegionId++;
            }
        }

        return regions.ToArray();
    }

    private static void FillRegion(WorldData world, HexData startHex, Region newRegion)
    {
        Biome targetBiome = startHex.ExtraData.Biome;

        Stack<HexData> stack = new Stack<HexData>();
        stack.Push(startHex);

        while (stack.Count > 0)
        {
            HexData hex = stack.Pop();
            newRegion.Size++;

            if (hex == null) continue;
            if (hex.ExtraData.RegionId != 0) continue;
            if (hex.ExtraData.Biome != targetBiome) continue;

            hex.ExtraData.SetRegionID(newRegion.ID);

            foreach (HexData neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, hex, 1))
            {
                if (neighbor == null) continue;
                if (neighbor.ExtraData.RegionId != 0) continue;
                if (neighbor.ExtraData.Biome != targetBiome) continue;

                stack.Push(neighbor);
            }
        }
    }
}
