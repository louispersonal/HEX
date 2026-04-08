using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverGen
{
    private const float MINIMUM_ELEVATION_RIVER_SOURCE = 0.5f;
    private const float MINIMUM_PRECIPITATION_RIVER_SOURCE = 0.5f;
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
            }
        }
    }

    private static bool RiverOriginViability(HexData hex)
    {
        return (hex.ExtraData.Elevation > MINIMUM_ELEVATION_RIVER_SOURCE
            && hex.ExtraData.Precipitation > MINIMUM_PRECIPITATION_RIVER_SOURCE);
    }

}
