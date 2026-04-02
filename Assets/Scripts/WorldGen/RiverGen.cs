using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverGen
{
    private const float MINIMUM_ELEVATION_RIVER_SOURCE = 0.5f;
    private const float MINIMUM_PRECIPITATION_RIVER_SOURCE = 0.5f;
    private const int MINIMUM_RIVER_LENGTH = 5;
    private const int MAXIMUM_RIVER_LENGTH = 30;

    public static void GenerateRivers(WorldData world)
    {
        int riverIndex = 0;
        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (RiverOriginViability(data))
            {
                RiverID newID = new RiverID(riverIndex);
                River newRiver = new River(newID, data.Coord);
                riverIndex++;

                BuildRiver(world, newRiver);
            }
        }
    }

    private static bool RiverOriginViability(HexData hex)
    {
        return (hex.ExtraData.Elevation > MINIMUM_ELEVATION_RIVER_SOURCE
            && hex.ExtraData.Precipitation > MINIMUM_PRECIPITATION_RIVER_SOURCE);
    }

    private static void FindMouth(WorldData world, River river)
    {
        for (int radius = MINIMUM_RIVER_LENGTH; radius <= MAXIMUM_RIVER_LENGTH; radius++)
        {
            HexData sourceHex = world.Grid.TryGetHex(river.Source, out HexData outHex) ? outHex : null;
            foreach (HexData hex in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, sourceHex, radius))
            { 

            }
        }

    }

    private static void BuildRiver(WorldData world, River river)
    {
        Pathfinder pathFinder = new Pathfinder();
        pathFinder.HexGrid = world.Grid;
    }
}
