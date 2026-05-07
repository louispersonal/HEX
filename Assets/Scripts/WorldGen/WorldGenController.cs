using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenController : MonoBehaviour
{
    [SerializeField] WorldGenParameters _worldParams;

    [SerializeField] List<GeoFeatureSpawnParameters> _geoFeatureSpawnParameters;

    private const float BASE_FREQUENCY = 0.015f;
    private const int OCTAVES = 6;
    private const float LACUNARITY = 1.9f;
    private const float GAIN = 0.8f;
    private const float WIDTH_HEIGHT_RATIO = 1.5f;
    private const int FRACTAL_WIDTH_SPAN = 200;

    private bool _generationInProgress = false;
    public bool GenerationInProgress { get { return _generationInProgress; } }

    private WorldData _newWorld;
    public WorldData NewWorld { get { return _newWorld; } }

    private string _currentStatus;
    public string  CurrentStatus { get { return _currentStatus; } }
    private float _amountDone;
    public float AmountDone { get { return _amountDone; } }

    public IEnumerator GenerateWorldData(int worldWidthInHexes, int seed, string name)
    {
        _generationInProgress = true;
        _currentStatus = "Starting";
        _amountDone = 0f;

        FractalBrownianMotionParameters defaultParams = new FractalBrownianMotionParameters(BASE_FREQUENCY, OCTAVES, LACUNARITY, GAIN, FRACTAL_WIDTH_SPAN);

        List<HexData> newHexData = HexGridGeometry.GenerateRectangularGrid(worldWidthInHexes, Mathf.RoundToInt(worldWidthInHexes / WIDTH_HEIGHT_RATIO));

        _newWorld = new WorldData(newHexData);

        _newWorld.Name = name;

        _currentStatus = "Generating Heightmap";
        _amountDone = 0.16f;
        yield return null;

        ElevationGen.GenerateHeightmap(_newWorld.Grid, seed, defaultParams, WIDTH_HEIGHT_RATIO);

        _currentStatus = "Computing Temperatures";
        _amountDone = 0.32f;
        yield return null;

        TemperatureGen.ComputeTemperatures(_newWorld.Grid, _worldParams);

        _currentStatus = "Computing Precipitations";
        _amountDone = 0.48f;
        yield return null;

        PrecipitationGen.ComputePrecipitations(_newWorld.Grid, _worldParams);

        _currentStatus = "Generating Rivers";
        _amountDone = 0.6f;
        yield return null;

        RiverGen.GenerateRivers(_newWorld, _worldParams);

        _currentStatus = "Generating Vegetation";
        _amountDone = 0.78f;
        yield return null;

        VegetationGen.GenerateVegetation(_newWorld.Grid, _worldParams);

        GeoFeatureGen.AddGeoFeatures(_newWorld, _geoFeatureSpawnParameters);

       _newWorld.Regions = RegionGen.CreateRegions(_newWorld, _worldParams);

        _currentStatus = "Done";
        _amountDone = 1f;
        yield return null;

        DebugStats(_newWorld);

        _generationInProgress = false;
        yield return null;
    }

    private void DebugStats(WorldData world)
    {
        float highestElevation = 0f;
        float highestTemperature = 0f;
        float highestPrecipitation = 0f;
        float strongestWind = 0f;

        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (data.ExtraData.Elevation > highestElevation) highestElevation = data.ExtraData.Elevation;
            if (data.ExtraData.Temperature > highestTemperature) highestTemperature = data.ExtraData.Temperature;
            if (data.ExtraData.Precipitation > highestPrecipitation) highestPrecipitation = data.ExtraData.Precipitation;
            if (world.Grid.GetWindDirection(data.Coord).magnitude > strongestWind) strongestWind = world.Grid.GetWindDirection(data.Coord).magnitude;
        }

        Debug.Log($"Max Elevation: {highestElevation}, Max Temperature: {highestTemperature}, Max Precipitation: {highestPrecipitation}, Strongest Wind: {strongestWind}");
    }
}
