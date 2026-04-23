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
                FillRegion(world, data, newRegion);
                newRegion.Size = newRegion.GetHexesInRegion(world).Count;
                regions.Add(newRegion);
                currentRegionId++;
            }
        }

        return regions.ToArray();
    }

    private static void FillRegion(WorldData world, HexData seedHex, Region newRegion)
    {
        seedHex.ExtraData.SetRegionID(newRegion.ID);
        foreach (HexData neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, seedHex, 1))
        {
            if (neighbor.ExtraData.RegionId == 0 && neighbor.ExtraData.Biome == seedHex.ExtraData.Biome)
            {
                FillRegion(world, neighbor, newRegion);
            }
        }
    }
}
