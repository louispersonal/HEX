using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RiverGen
{
    public static void GenerateRivers(WorldData world, WorldGenParameters parameters)
    {
        int riverIndex = 0;
        int lakeIndex = 0;
        foreach (HexData data in world.Grid.GetValidHexes())
        {
            float riverChance = 1f / (parameters.RiverSpacing * world.Grid.Width);
            if (RiverOriginViability(data, parameters, riverChance))
            {
                RiverID newID = new RiverID(riverIndex);
                River newRiver = new River(newID, data.Coord);

                BuildRiver(newRiver, world, parameters, out HexData lakeHex);

                world.Rivers.Add(newID, newRiver, newRiver.Coords);
                riverIndex++;

                if (lakeHex != null)
                {
                    LakeID newLakeId = new LakeID(lakeIndex);
                    Lake newLake = new Lake(newLakeId, new List<AxialCoordinate> { lakeHex.Coord });
                    world.Lakes.Add(newLakeId, newLake, new List<AxialCoordinate> { lakeHex.Coord });
                    lakeIndex++;
                }
            }
        }
    }

    private static bool RiverOriginViability(HexData hex, WorldGenParameters parameters, float riverChance)
    {
        return (hex.ExtraData.Elevation > parameters.MinimumElevationRiverSource
            && hex.ExtraData.Precipitation > parameters.MinimumPrecipitationRiverSource
            && Random.Range(0f, 1f) < riverChance);
    }

    private static void BuildRiver(River newRiver, WorldData world, WorldGenParameters parameters, out HexData lakeHex)
    {
        int riverLength = 1;
        AxialCoordinate currentCoord = newRiver.Source;
        bool endsInLake = false;
        lakeHex = null;
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

            if (downHillNeighbor == currentHex) { endsInLake = true; break; }
            if (world.Rivers.ContainsAt(downHillNeighbor.Coord)) break;

            newRiver.Coords.Add(downHillNeighbor.Coord);

            currentCoord = downHillNeighbor.Coord;

            riverLength++;
        }

        if (endsInLake && riverLength > 1)
        {
            lakeHex = world.Grid.TryGetHex(currentCoord, out HexData hex)? hex : null;
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
