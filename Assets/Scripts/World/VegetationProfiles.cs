using System;
using System.Collections.Generic;

public static class VegetationProfiles
{
    public static readonly BiomePlantProfile DesertProfile = new()
    {
        LowVProfile = new PlantProfile(
            greens: 0.18f,
            seeds:   0.25f,
            fruit:   0.08f,
            roots:   0.28f,
            wood:    0.10f,
            grubs:   0.09f,
            fungus:  0.02f
        ),

        HighVProfile = new PlantProfile(
            greens: 0.12f,
            seeds:   0.18f,
            fruit:   0.15f,
            roots:   0.08f,
            wood:    0.38f,
            grubs:   0.07f,
            fungus:  0.02f
        )
    };

    public static readonly BiomePlantProfile TundraProfile = new()
    {
        LowVProfile = new PlantProfile(
            greens: 0.30f,
            seeds:   0.08f,
            fruit:   0.12f,
            roots:   0.18f,
            wood:    0.05f,
            grubs:   0.12f,
            fungus:  0.15f
        ),

        HighVProfile = new PlantProfile(
            greens: 0.18f,
            seeds:   0.08f,
            fruit:   0.14f,
            roots:   0.07f,
            wood:    0.28f,
            grubs:   0.10f,
            fungus:  0.15f
        )
    };

    public static readonly BiomePlantProfile TaigaProfile = new()
    {
        LowVProfile = new PlantProfile(
            greens: 0.22f,
            seeds:   0.10f,
            fruit:   0.12f,
            roots:   0.10f,
            wood:    0.08f,
            grubs:   0.16f,
            fungus:  0.22f
        ),

        HighVProfile = new PlantProfile(
            greens: 0.14f,
            seeds:   0.13f,
            fruit:   0.08f,
            roots:   0.04f,
            wood:    0.42f,
            grubs:   0.08f,
            fungus:  0.11f
        )
    };

    public static readonly BiomePlantProfile TropicalProfile = new()
    {
        LowVProfile = new PlantProfile(
            greens: 0.25f,
            seeds:   0.08f,
            fruit:   0.15f,
            roots:   0.10f,
            wood:    0.06f,
            grubs:   0.21f,
            fungus:  0.15f
        ),

        HighVProfile = new PlantProfile(
            greens: 0.14f,
            seeds:   0.08f,
            fruit:   0.22f,
            roots:   0.03f,
            wood:    0.34f,
            grubs:   0.12f,
            fungus:  0.07f
        )
    };

    public static readonly BiomePlantProfile SavannaProfile = new()
    {
        LowVProfile = new PlantProfile(
            greens: 0.42f,
            seeds:   0.20f,
            fruit:   0.06f,
            roots:   0.12f,
            wood:    0.04f,
            grubs:   0.13f,
            fungus:  0.03f
        ),

        HighVProfile = new PlantProfile(
            greens: 0.18f,
            seeds:   0.10f,
            fruit:   0.18f,
            roots:   0.05f,
            wood:    0.36f,
            grubs:   0.09f,
            fungus:  0.04f
        )
    };

    public static readonly BiomePlantProfile TemperateProfile = new()
    {
        LowVProfile = new PlantProfile(
            greens: 0.27f,
            seeds:   0.14f,
            fruit:   0.12f,
            roots:   0.12f,
            wood:    0.07f,
            grubs:   0.14f,
            fungus:  0.14f
        ),

        HighVProfile = new PlantProfile(
            greens: 0.13f,
            seeds:   0.18f,
            fruit:   0.14f,
            roots:   0.03f,
            wood:    0.36f,
            grubs:   0.07f,
            fungus:  0.09f
        )
    };

    public static readonly BiomePlantProfile SteppeProfile = new()
    {
        LowVProfile = new PlantProfile(
            greens: 0.43f,
            seeds:   0.24f,
            fruit:   0.04f,
            roots:   0.14f,
            wood:    0.03f,
            grubs:   0.10f,
            fungus:  0.02f
        ),

        HighVProfile = new PlantProfile(
            greens: 0.17f,
            seeds:   0.15f,
            fruit:   0.10f,
            roots:   0.06f,
            wood:    0.40f,
            grubs:   0.08f,
            fungus:  0.04f
        )
    };

    public static readonly IReadOnlyDictionary<Biome, BiomePlantProfile> Profiles =
        new Dictionary<Biome, BiomePlantProfile>
        {
            [Biome.Desert] = DesertProfile,
            [Biome.Tundra] = TundraProfile,
            [Biome.Taiga] = TaigaProfile,
            [Biome.Savanna] = SavannaProfile,
            [Biome.Temperate] = TemperateProfile,
            [Biome.Steppe] = SteppeProfile,
            [Biome.Tropical] = TropicalProfile
        };
}

public class BiomePlantProfile
{
    public PlantProfile LowVProfile;
    public PlantProfile HighVProfile;
}

public class PlantProfile
{
    private const int ItemCount = 7;
    private const float PrecisionTolerance = 0.01f;

    public readonly float[] Items = new float[ItemCount];

    public PlantProfile(
        float greens,
        float seeds,
        float fruit,
        float roots,
        float wood,
        float grubs,
        float fungus)
    {
        float total =
            greens +
            seeds +
            fruit +
            roots +
            wood +
            grubs +
            fungus;

        if (Math.Abs(total - 1f) > PrecisionTolerance)
        {
            throw new ArgumentException($"Plant profile fractions must total 1.0. Actual total: {total}");
        }

        Items[(int)PlantProfileIndexes.Greens] = greens;
        Items[(int)PlantProfileIndexes.Seeds] = seeds;
        Items[(int)PlantProfileIndexes.Fruit] = fruit;
        Items[(int)PlantProfileIndexes.Roots] = roots;
        Items[(int)PlantProfileIndexes.Wood] = wood;
        Items[(int)PlantProfileIndexes.Grubs] = grubs;
        Items[(int)PlantProfileIndexes.Fungus] = fungus;
    }

    public float this[PlantProfileIndexes index] => Items[(int)index];
}

public enum PlantProfileIndexes
{
    Greens,
    Seeds,
    Fruit,
    Roots,
    Wood,
    Grubs,
    Fungus
}