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
public class ArchetypeProfile
{
    public ushort ArchetypeId;

    public string CommonName;

    public SizeTier Size;

    public Resources[] Eats;

    public float NutritionRequirement
    {
        get
        {
            switch (Size)
            {
                case SizeTier.Small:
                    return 1;
                case SizeTier.Medium:
                    return 10;
                default:
                    return 100;
            }
        }
    }
}

[System.Serializable]
public class SpeciesProfile
{
    public ushort ArchetypeId;
    public ushort SpeciesId;
    public string SpeciesName;
    public List<AnimalTags> Tags;
}

public enum SizeTier
{
    Small = 0,
    Medium = 1,
    Large = 2
}

public enum AnimalTags
{
    Venomous,
    Poisonous,
    Furry,
    Woolly,
    Flying,
    Scales,
    Fast
}