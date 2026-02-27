using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureGen
{
    public static void ComputeTemperatures(HexGrid grid)
    {
        Dictionary<AxialCoordinate, float> baseTemps = new Dictionary<AxialCoordinate, float>();
        Dictionary<AxialCoordinate, float> windAdjustedTemps = new Dictionary<AxialCoordinate, float>();
        foreach (HexData data in grid.Grid.Values)
        {
            float latitude = grid.GetLatitude01(data.Coord);
            baseTemps[data.Coord] = ComputeBaseTemperature(latitude, data.ExtraData.Elevation, data.Coord);
        }

        foreach (HexData data in grid.Grid.Values)
        {
            float coastalDistance = grid.NumHexesFromSea(data, out HexData seaHex);
            if (coastalDistance > 0 && seaHex != null)
            {
                float coastalFactor = 1f / (1f + (coastalDistance / 8f));
                baseTemps[data.Coord] = Mathf.Lerp(baseTemps[data.Coord], baseTemps[seaHex.Coord], coastalFactor * 0.6f);
            }
        }

        for (int windPasses = 0; windPasses < 4; windPasses++)
        {
            foreach (HexData data in grid.Grid.Values)
            {
                Vector2 windDirection = grid.GetWindDirection(data.Coord);
                Vector2 upWindDirection = -windDirection;
                AxialCoordinate neighborCoord = data.Coord + AxialGeometry.ConvertVectorToAxialDirection(upWindDirection);
                if (grid.TryGetHex(neighborCoord, out HexData neighborHex))
                {
                    float neighborTemp = baseTemps[neighborCoord];
                    float newTemp = Mathf.Lerp(baseTemps[data.Coord], neighborTemp, Mathf.Abs(windDirection.magnitude));
                    windAdjustedTemps[data.Coord] = newTemp;
                }
            }

            foreach (HexData data in grid.Grid.Values)
            {
                if (windAdjustedTemps.TryGetValue(data.Coord, out float windAdjustedTemp))
                {
                    baseTemps[data.Coord] = windAdjustedTemp;
                }
            }
        }

        foreach (HexData data in grid.Grid.Values)
        {
            data.ExtraData.SetTemperature(baseTemps[data.Coord]);
        }
    }

    public static float ComputeBaseTemperature(float latitude, float elevation, AxialCoordinate coord)
    {
        float baseTemp = (1 - latitude) - (elevation * 0.128532f);
        float noiseSize = 0.3f;
        float noiseSampleRate = 0.02f;
        Vector2 noiseSamplePoint = AxialGeometry.AxialToCartesian(coord, noiseSampleRate);
        float noise = Mathf.PerlinNoise(noiseSamplePoint.x, noiseSamplePoint.y);
        noise = (noise * 2f) - 1f;
        return Mathf.Clamp01(baseTemp + (noiseSize * noise));
    }
}
