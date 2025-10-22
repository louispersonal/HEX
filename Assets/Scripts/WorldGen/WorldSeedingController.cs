using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;

public class WorldSeedingController
{
    private const int MAX_HEXES_FROM_WATER = 10;

    public static BaseHexGrid SeedWorld(BaseHexGrid hexGrid, MapDefinition mapDefinition)
    {
        SetTemperatures(hexGrid, mapDefinition);
        SetPrecipitations(hexGrid);
        SetVegetations(hexGrid);

        return hexGrid;
    }

    public static void SetTemperatures(BaseHexGrid hexGrid, MapDefinition mapDefinition)
    {
        foreach (Hex hex in hexGrid.Grid.Values)
        {
            float y = hex.Coord.R;
            float value = 1f - Normalize(Mathf.Abs(y - mapDefinition.FBMParams.WorldHeight/ 2), 0f, mapDefinition.FBMParams.WorldHeight / 2);
            hex.SetTemperature(value);
        }
    }

    public static void SetPrecipitations(BaseHexGrid hexGrid)
    {
        foreach (Hex hex in hexGrid.Grid.Values)
        {
            int distance = DistanceToNearestWaterHex(hex);
            float value = 1f - Normalize(distance, 0f, MAX_HEXES_FROM_WATER);
            hex.SetPrecipitation(value);
        }
    }

    public static int DistanceToNearestWaterHex(Hex hex)
    {
        for (int r = 0; r < MAX_HEXES_FROM_WATER; r++)
        {
            List<Hex> hexes = hex.HexesInRingOfRadius(r);
            foreach (Hex target in hexes)
            {
                if (target.IsSea) return r;
            }
        }

        return MAX_HEXES_FROM_WATER;
    }

    public static void SetVegetations(BaseHexGrid hexGrid)
    {
        foreach (Hex hex in hexGrid.Grid.Values)
        {
            float lowVegetation = SmoothThresholdFunction(hex.Temperature, 0.1f, 0.9f)
                * SmoothThresholdFunction(hex.Precipitation, 0.1f, 0.9f)
                * (1 - SmoothThresholdFunction(hex.Elevation, 0.9f, 1f));

            float highVegetation = SmoothThresholdFunction(hex.Temperature, 0.4f, 0.9f)
                * SmoothThresholdFunction(hex.Precipitation, 0.4f, 0.9f)
                * (1 - SmoothThresholdFunction(hex.Elevation, 0.7f, 1f));

            hex.SetVegetations(lowVegetation, highVegetation);
        }
    }

    public static float SmoothThresholdFunction(float value, float floor, float ceiling)
    {
        return Mathf.Clamp((value - floor) / (ceiling - floor), 0, 1);
    }

    private static float Normalize(float v, float floor, float ceiling)
    {
        return (v - floor) / (ceiling - floor);
    }

    public static float ClampedOptimumCurve(float tightness, float optimum, float val)
    {
        if (tightness < 0 || tightness > 1) Debug.LogError("tightness out of bounds (0, 1)");
        if (optimum < 0 || optimum > 1) Debug.LogError("optimum out of bounds (0, 1)");

        return Mathf.Max(0f, 1 - (tightness * 100) * Mathf.Pow(val - optimum, 2));
    }
}
