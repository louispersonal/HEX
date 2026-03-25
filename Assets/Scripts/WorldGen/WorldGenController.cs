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

        return newWorld;
    }
}
