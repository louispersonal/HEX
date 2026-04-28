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
                FillRegion(world, data, newRegion, maxRegionSize);
                regions.Add(newRegion);
                currentRegionId++;
            }
        }

        return regions.ToArray();
    }

    private static void FillRegion(WorldData world, HexData startHex, Region newRegion, int maxRegionSize)
    {
        Biome targetBiome = startHex.ExtraData.Biome;

        Queue<HexData> queue = new Queue<HexData>();

        startHex.ExtraData.SetRegionID(newRegion.ID);
        queue.Enqueue(startHex);

        while (queue.Count > 0 && newRegion.Size < maxRegionSize)
        {
            HexData hex = queue.Dequeue();
            newRegion.Size++;

            foreach (HexData neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, hex, 1))
            {
                if (neighbor == null) continue;
                if (neighbor.ExtraData.RegionId != 0) continue;
                if (neighbor.ExtraData.Biome != targetBiome) continue;

                neighbor.ExtraData.SetRegionID(newRegion.ID);
                queue.Enqueue(neighbor);
            }
        }
    }
}
