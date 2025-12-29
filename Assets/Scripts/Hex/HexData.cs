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

    }

    public HexData(int q, int r) : base(q, r)
    {
        
    }

    public override string ToString()
    {
        return $"Grass and shrubs: {_extraData.LowVegetation},\n" +
            $"Trees: {_extraData.HighVegetation},\n" +
            $"Temperature: {_extraData.Temperature * 150 - 90},\n" +
            $"Elevation: {_extraData.Elevation * 9000}m,\n" +
            $"Precipitation: {_extraData.Precipitation}\n" +
            $"Biome: {_extraData.Biome}";
    }
}

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
}