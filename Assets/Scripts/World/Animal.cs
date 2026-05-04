using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGroup
{
    public ushort SpeciesId;

    public ushort RegionId;
    public int Count;
}

[System.Serializable]
public class SpeciesProfile
{
    public ushort SpeciesId;

    public string CommonName;

    public SizeTier Size;

    public FoodType[] Eats;
    public Biome PreferredBiome;
}

public enum SizeTier
{
    Small = 0,
    Medium = 1,
    Large = 2,
    Huge = 3
}

public enum FoodType
{
    LowVegetation = 0,
    HighVegetation = 1,
    Microfauna = 2,
    Meat = 3,
    Fish = 4,
    Carrion = 5
}