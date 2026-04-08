using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenController
{
    private const float BASE_FREQUENCY = 0.015f;
    private const int OCTAVES = 6;
    private const float LACUNARITY = 1.9f;
    private const float GAIN = 0.8f;
    private const float WIDTH_HEIGHT_RATIO = 1.5f;
    private const int FRACTAL_WIDTH_SPAN = 200;

    public static WorldData GenerateWorldData(int worldWidthInHexes, int seed)
    {
        FractalBrownianMotionParameters defaultParams = new FractalBrownianMotionParameters(BASE_FREQUENCY, OCTAVES, LACUNARITY, GAIN, FRACTAL_WIDTH_SPAN);

        List<HexData> newHexData = HexGridGeometry.GenerateRectangularGrid(worldWidthInHexes, Mathf.RoundToInt(worldWidthInHexes / WIDTH_HEIGHT_RATIO));

        WorldData newWorld = new WorldData(newHexData);

        ElevationGen.GenerateHeightmap(newWorld.Grid, seed, defaultParams, WIDTH_HEIGHT_RATIO);

        TemperatureGen.ComputeTemperatures(newWorld.Grid);

        PrecipitationGen.ComputePrecipitations(newWorld.Grid);

        RiverGen.GenerateRivers(newWorld);

        DebugStats(newWorld);

        return newWorld;
    }

    private static void DebugStats(WorldData world)
    {
        float highestElevation = 0f;
        float highestTemperature = 0f;
        float highestPrecipitation = 0f;

        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (data.ExtraData.Elevation > highestElevation) highestElevation = data.ExtraData.Elevation;
            if (data.ExtraData.Temperature > highestTemperature) highestTemperature = data.ExtraData.Temperature;
            if (data.ExtraData.Precipitation > highestPrecipitation) highestPrecipitation = data.ExtraData.Precipitation;
        }

        Debug.Log($"Max Elevation: {highestElevation}, Max Temperature: {highestTemperature}, Max Precipitation: {highestPrecipitation}");
    }
}
