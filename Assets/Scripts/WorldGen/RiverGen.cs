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

        List<HexData> candidateHexes = new List<HexData>();

        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (RiverOriginViability(data, parameters)) candidateHexes.Add(data);

        }

        int numCandidates = candidateHexes.Count;
        float chance = parameters.TargetNumberRivers / numCandidates;

        foreach (HexData candidate in candidateHexes)
        {
            if (Random.Range(0f, 1f) < chance)
            {
                RiverID newID = new RiverID(riverIndex);
                River newRiver = new River(newID, candidate.Coord);

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

    private static bool RiverOriginViability(HexData hex, WorldGenParameters parameters)
    {
        return (hex.ExtraData.Elevation > parameters.MinimumElevationRiverSource
            || hex.ExtraData.Precipitation > parameters.MinimumPrecipitationRiverSource);
    }

    private static void BuildRiver(
        River newRiver,
        WorldData world,
        WorldGenParameters parameters,
        out HexData lakeHex)
    {
        int riverLength = 1;
        AxialCoordinate currentCoord = newRiver.Source;

        lakeHex = null;

        float uphillTolerance = 0.1f;
        int flatStepsRemaining = 5;

        while (riverLength < world.Grid.Width / parameters.MaximumRiverLengthRatio)
        {
            if (CheckAdjacentSea(currentCoord, world))
                break;

            if (!world.Grid.TryGetHex(currentCoord, out HexData currentHex))
                break;

            HexData bestDownhill = null;
            HexData bestTolerated = null;

            float currentElevation = currentHex.ExtraData.Elevation;
            float lowestDownhillElevation = currentElevation;
            float lowestToleratedElevation = currentElevation + uphillTolerance;

            foreach (HexData neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, currentHex, 1))
            {
                if (neighbor == null) continue;
                if (world.Rivers.ContainsAt(neighbor.Coord)) continue;

                float neighborElevation = neighbor.ExtraData.Elevation;

                // True downhill candidate
                if (neighborElevation < lowestDownhillElevation)
                {
                    bestDownhill = neighbor;
                    lowestDownhillElevation = neighborElevation;
                }

                // Slightly uphill / flat escape candidate
                if (neighborElevation <= lowestToleratedElevation)
                {
                    bestTolerated = neighbor;
                    lowestToleratedElevation = neighborElevation;
                }
            }

            HexData nextHex = null;

            if (bestDownhill != null)
            {
                nextHex = bestDownhill;
                flatStepsRemaining = 3;
            }
            else if (bestTolerated != null && flatStepsRemaining > 0)
            {
                nextHex = bestTolerated;
                flatStepsRemaining--;
            }
            else
            {
                lakeHex = currentHex;
                break;
            }

            newRiver.Coords.Add(nextHex.Coord);
            currentCoord = nextHex.Coord;
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
