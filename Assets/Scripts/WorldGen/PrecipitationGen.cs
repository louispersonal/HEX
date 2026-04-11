using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecipitationGen
{
    public static void ComputePrecipitations(HexGrid grid, WorldGenParameters parameters)
    {
        Dictionary<AxialCoordinate, float> baseHums = new Dictionary<AxialCoordinate, float>();
        Dictionary<AxialCoordinate, float> windAdjustedHums = new Dictionary<AxialCoordinate, float>();

        // Seed base values (sea 1, land 0)
        foreach (HexData data in grid.GetValidHexes())
        {
            baseHums[data.Coord] = 0f;
            if (data.ExtraData.IsSea) baseHums[data.Coord] = 1f;
        }

        for (int windPasses = 0; windPasses < parameters.HumidityPasses; windPasses++)
        {
            foreach (HexData data in grid.GetValidHexes())
            {
                Vector2 windDirection = grid.GetWindDirection(data.Coord);
                Vector2 upWindDirection = -windDirection;
                AxialCoordinate upwindCoord = data.Coord + AxialGeometry.ConvertVectorToAxialDirection(upWindDirection);
                if (baseHums.TryGetValue(upwindCoord, out float upwindHum))
                {
                    float newHum = upwindHum * parameters.HumidityTransferScalar;

                    windAdjustedHums[data.Coord] = newHum;
                }
            }

            foreach (HexData data in grid.GetValidHexes())
            {
                if (windAdjustedHums.TryGetValue(data.Coord, out float windAdjustedHum))
                {
                    baseHums[data.Coord] = windAdjustedHum;
                }
            }
        }

        foreach (HexData data in grid.GetValidHexes())
        {
            data.ExtraData.SetPrecipitation(baseHums[data.Coord]);
        }
    }
}
