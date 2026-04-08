using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RiverGen
{
    private const float MINIMUM_ELEVATION_RIVER_SOURCE = 0.5f;
    private const float MINIMUM_PRECIPITATION_RIVER_SOURCE = 0.5f;
    private const int MAXIMUM_RIVER_LENGTH = 100;
    private const float BASE_CHANCE = 0.1f;

    public static void GenerateRivers(WorldData world)
    {
        int riverIndex = 0;
        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (RiverOriginViability(data))
            {
                RiverID newID = new RiverID(riverIndex);
                River newRiver = new River(newID, data.Coord);

                BuildRiver(newRiver, world);

                AddRiverToWorld(newRiver, world);
                riverIndex++;
            }
        }
    }

    private static bool RiverOriginViability(HexData hex)
    {
        return (hex.ExtraData.Elevation > MINIMUM_ELEVATION_RIVER_SOURCE
            && hex.ExtraData.Precipitation > MINIMUM_PRECIPITATION_RIVER_SOURCE
            && Random.Range(0f, 1f) < BASE_CHANCE);
    }

    private static void BuildRiver(River newRiver, WorldData world)
    {
        int riverLength = 1;
        AxialCoordinate currentCoord = newRiver.Source;
        while (riverLength < MAXIMUM_RIVER_LENGTH)
        {
            if (CheckAdjacentSea(currentCoord, world)) break;

            // pick most downhill neighbor
            world.Grid.TryGetHex(currentCoord, out HexData currentHex);
            HexData downHillNeighbor = currentHex;
            foreach (HexData neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, currentHex, 1))
            {
                if (neighbor.ExtraData.Elevation < downHillNeighbor.ExtraData.Elevation) downHillNeighbor = neighbor;
            }

            if (downHillNeighbor == currentHex) break;

            newRiver.Hexes.Add(downHillNeighbor.Coord);

            currentCoord = downHillNeighbor.Coord;

            riverLength++;
        }
        newRiver.Mouth = currentCoord;
    }

    private static bool CheckAdjacentSea(AxialCoordinate coord, WorldData world)
    {
        world.Grid.TryGetHex(coord, out HexData currentHex);
        foreach (HexData neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, currentHex, 1))
        {
            if (neighbor.ExtraData.IsSea) return true;
        }
        return false;
    }

    private static void AddRiverToWorld(River newRiver, WorldData world)
    {
       world.Rivers.Add(newRiver.ID, newRiver);
       foreach(AxialCoordinate coord in newRiver.Hexes)
        {
            world.RiverLookup.TryAdd(coord, newRiver.ID);
        }
    }
}
