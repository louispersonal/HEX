using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGroup
{
    public ushort ArchetypeId;

    public ushort RegionId;
    public int Count;
}

[System.Serializable]
public class ArchetypeProfile
{
    public ushort ArchetypeId;

    public string CommonName;

    public SizeTier Size;

    public FoodType[] Eats;

    public int MaxPopulationPerHex; // assuming maximum food abundance

    public bool FitsInRegion(WorldData data, Region region)
    {
        bool canEat;
        return region.HasRiver(data);
    }
}

public enum SizeTier
{
    Small = 0,
    Medium = 1,
    Large = 2
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