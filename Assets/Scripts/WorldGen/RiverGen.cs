using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RiverGen
{
    public static void GenerateRivers(WorldData world, WorldGenParameters parameters)
    {
        int riverIndex = 0;
        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (RiverOriginViability(data, parameters))
            {
                RiverID newID = new RiverID(riverIndex);
                River newRiver = new River(newID, data.Coord);

                BuildRiver(newRiver, world, parameters);

                world.Rivers.Add(newID, newRiver, newRiver.Coords);
                riverIndex++;
            }
        }
    }

    private static bool RiverOriginViability(HexData hex, WorldGenParameters parameters)
    {
        return (hex.ExtraData.Elevation > parameters.MinimumElevationRiverSource
            && hex.ExtraData.Precipitation > parameters.MinimumPrecipitationRiverSource
            && Random.Range(0f, 1f) < parameters.BaseRiverChance);
    }

    private static void BuildRiver(River newRiver, WorldData world, WorldGenParameters parameters)
    {
        int riverLength = 1;
        AxialCoordinate currentCoord = newRiver.Source;
        while (riverLength < world.Grid.Width / parameters.MaximumRiverLengthRatio)
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

            newRiver.Coords.Add(downHillNeighbor.Coord);

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
}
