using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hex : BaseHex
{
    private const float ByteToFloat = 1f / 255f;

    [SerializeField] byte _lowVegetation;
    public float LowVegetation { get { return _lowVegetation * ByteToFloat; } }

    [SerializeField] byte _highVegetation;
    public float HighVegetation { get { return _highVegetation * ByteToFloat; } }

    [SerializeField] byte _temperature;
    public float Temperature { get { return _temperature * ByteToFloat; } }

    [SerializeField] byte _elevation;
    public float Elevation { get { return _elevation * ByteToFloat; } }

    [SerializeField] byte _precipitation;
    public float Precipitation { get { return _precipitation * ByteToFloat; } }

    public bool IsSea { get { return _elevation == 0; } }

    public Biome Biome { get { return Biomes.GetBiome(Temperature, Precipitation); } }

    public Hex(AxialCoordinate a) : base(a)
    {

    }

    public Hex(int q, int r) : base(q, r)
    {
        
    }

    public override string ToString()
    {
        return $"Grass and shrubs: {LowVegetation},\n" +
            $"Trees: {HighVegetation},\n" +
            $"Temperature: {Temperature * 150 - 90},\n" +
            $"Elevation: {Elevation * 9000}m,\n" +
            $"Precipitation: {Precipitation}\n" +
            $"Biome: {Biome}";
    }

    public List<Hex> GetNeighbors()
    {
        List<Hex> neighbors = new List<Hex>();

        foreach (AxialCoordinate dir in AxialDirections.Directions)
        {
            AxialCoordinate neighborCoord = Coord + dir;
            if (BaseHexGrid.Instance.TryGetHex(neighborCoord, out Hex neighborHex))
            {
                neighbors.Add(neighborHex);
            }
        }

        return neighbors;
    }

    public float DistanceToHex(Hex target)
    {
        return BaseHexGrid.Instance.DistanceBetweenHexes(this, target);
    }

    public List<Hex> HexesWithinRadius(int radius)
    {
        return BaseHexGrid.Instance.HexesWithinRadiusOfHex(this, radius);
    }

    public List<Hex> HexesInRingOfRadius(int radius)
    {
        return BaseHexGrid.Instance.HexesInRingOfRadiusOfHex(this, radius);
    }

    public Vector2 GetScenePosition()
    {
        return BaseHexGrid.Instance.AxialToSceneConversion(Coord);
    }

    public void SetElevation(float elevation)
    {
        _elevation = (byte)(elevation * 255f);
    }

    public void SetTemperature(float temperature)
    {
        _temperature = (byte)(temperature * 255f);
    }

    public void SetPrecipitation(float precipitation)
    {
        _precipitation = (byte)(precipitation * 255f);
    }

    public void CalculateVegetations()
    {
        if (IsSea) return;

        WorldGenController w = WorldGenController.Instance;

        float lowVegetation = SmoothThresholdFunction(Temperature, 0.1f, 0.9f)
            * SmoothThresholdFunction(Precipitation, 0.1f, 0.9f)
            * (1 - SmoothThresholdFunction(Elevation, 0.9f, 1f));

        float highVegetation = SmoothThresholdFunction(Temperature, 0.4f, 0.9f)
            * SmoothThresholdFunction(Precipitation, 0.4f, 0.9f)
            * (1 - SmoothThresholdFunction(Elevation, 0.7f, 1f));

        _lowVegetation = (byte)(lowVegetation * 255f);
        _highVegetation = (byte)(highVegetation * 255f);
    }

    public float SmoothThresholdFunction(float value, float floor, float ceiling)
    {
        return Mathf.Clamp((value - floor) / (ceiling - floor), 0, 1);
    }
}
