using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biome
{
    Desert,
    Tundra,
    Taiga,
    Tropical,
    Savanna,
    Temperate,
    Steppe
}

public class Biomes
{
    public static float[] TemperatureThresholds = { 0.33f, 0.66f };
    public static float[] PrecipitationThresholds = { 0.33f, 0.66f };

    // [precipitation index, temperature index]
    public static Biome[,] BiomeTable = new Biome[3, 3]
    {
        { Biome.Tundra, Biome.Steppe, Biome.Desert },
        { Biome.Taiga, Biome.Temperate, Biome.Savanna },
        { Biome.Taiga, Biome.Temperate, Biome.Tropical }
    };

    public static int GetIndex(float value, float[] thresholds)
    {
        if (value < thresholds[0]) return 0;
        if (value < thresholds[1]) return 1;
        return 2;
    }

    public static Biome GetBiome(float temperature, float precipitation)
    {
        int tempIndex = GetIndex(temperature, TemperatureThresholds);
        int precipIndex = GetIndex(precipitation, PrecipitationThresholds);
        return BiomeTable[precipIndex, tempIndex];
    }
}