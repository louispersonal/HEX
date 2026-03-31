using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecipitationGen
{
    private static float _rainPasses = 14f;
    private static float _baseRain = 0.08f;

    public static void ComputePrecipitations(HexGrid grid)
    {
        Dictionary<AxialCoordinate, float> baseHums = new Dictionary<AxialCoordinate, float>();
        Dictionary<AxialCoordinate, float> windAdjustedHums = new Dictionary<AxialCoordinate, float>();
        Dictionary<AxialCoordinate, float> accumulatedPrecs = new Dictionary<AxialCoordinate, float>();

        foreach (HexData data in grid.GetValidHexes())
        {
            baseHums[data.Coord] = 0f;
            accumulatedPrecs[data.Coord] = 0f;
        }

        for (int windPasses = 0; windPasses < _rainPasses; windPasses++)
        {
            foreach (HexData data in grid.GetValidHexes())
            {
                Vector2 windDirection = grid.GetWindDirection(data.Coord);
                Vector2 upWindDirection = -windDirection;
                AxialCoordinate neighborCoord = data.Coord + AxialGeometry.ConvertVectorToAxialDirection(upWindDirection);
                if (baseHums.TryGetValue(neighborCoord, out float neighborHum))
                {
                    float newHum = Mathf.Lerp(baseHums[data.Coord], neighborHum, Mathf.Abs(windDirection.magnitude));

                    if (data.ExtraData.IsSea) // evaporate
                    {
                        newHum = Mathf.Lerp(newHum, 1f, data.ExtraData.Temperature);
                    }

                    else // precipitate
                    {
                        float rainThisStep = newHum > _baseRain ? _baseRain : 0f;

                        accumulatedPrecs[data.Coord] += rainThisStep;
                        newHum -= rainThisStep;
                    }

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

        float maxPrec = 0f;

        foreach (HexData data in grid.GetValidHexes())
        {
            float currentPrec = accumulatedPrecs[data.Coord];
            if (currentPrec > maxPrec) maxPrec = currentPrec;
        }

        foreach (HexData data in grid.GetValidHexes())
        {
            accumulatedPrecs[data.Coord] /= maxPrec;
        }

        foreach (HexData data in grid.GetValidHexes())
        {
            data.ExtraData.SetPrecipitation(accumulatedPrecs[data.Coord]);
        }
    }
}
