using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditor.Progress;

public class RiverGen
{
    public static void GenerateRivers(WorldData world, WorldGenParameters parameters)
    {
        int riverIndex = 0;
        int lakeIndex = 0;

        List<Hex> candidateHexes = new List<Hex>();

        foreach (Hex data in world.Grid.GetValidHexes())
        {
            if (RiverOriginViability(data, parameters)) candidateHexes.Add(data);
        }

        ListExtensions.Shuffle<Hex>(candidateHexes);

        for (int c = 0; c < parameters.TargetNumberRivers && c < candidateHexes.Count; c++)
        {
            RiverID newID = new RiverID(riverIndex);
            River newRiver = new River(newID, candidateHexes[c].Coord);

            BuildRiver(newRiver, world, parameters, out Hex lakeHex);
            newRiver.PopulateRiverConnections();
            
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

    private static bool RiverOriginViability(Hex hex, WorldGenParameters parameters)
    {
        return (hex.ExtraData.Elevation > parameters.MinimumElevationRiverSource
            || hex.ExtraData.Precipitation > parameters.MinimumPrecipitationRiverSource);
    }

    private static void BuildRiver(
        River newRiver,
        WorldData world,
        WorldGenParameters parameters,
        out Hex lakeHex)
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

            if (!world.Grid.TryGetHex(currentCoord, out Hex currentHex))
                break;

            Hex bestDownhill = null;
            Hex bestTolerated = null;

            float currentElevation = currentHex.ExtraData.Elevation;
            float lowestDownhillElevation = currentElevation;
            float lowestToleratedElevation = currentElevation + uphillTolerance;

            foreach (Hex neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, currentHex, 1))
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

            Hex nextHex = null;

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
    } 

    private static bool CheckAdjacentSea(AxialCoordinate coord, WorldData world)
    {
        world.Grid.TryGetHex(coord, out Hex currentHex);
        foreach (Hex neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, currentHex, 1))
        {
            if (neighbor.ExtraData.IsSea) return true;
        }
        return false;
    }
}