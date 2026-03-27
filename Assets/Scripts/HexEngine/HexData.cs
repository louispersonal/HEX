using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexData : BaseHexData
{
    public HexExtraData _extraData;
    public HexExtraData ExtraData { get { return _extraData; } }

    public HexData(AxialCoordinate a) : base(a)
    {
        _extraData = new HexExtraData();
    }

    public HexData(int q, int r) : base(q, r)
    {
        _extraData = new HexExtraData();
    }
}

[System.Serializable]
public class HexExtraData
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

    public Biome Biome { get { return Biomes.GetBiome(IsSea, Temperature, Precipitation); } }

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

    public void SetVegetations(float lowVegetation, float highVegetation)
    {
        _lowVegetation = (byte)(lowVegetation * 255f);
        _highVegetation = (byte)(highVegetation * 255f);
    }

    public int GetElevationInMeters()
    {
        return (int)(Elevation * 9000f);
    }

    public float GetTemperatureInDegrees()
    {
        return (Temperature * 70f) - 35f;
    }
}