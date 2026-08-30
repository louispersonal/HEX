using System;
using System.Collections.Generic;

public static class VegetationProfiles
{
    public static readonly BiomeVegetationProfile DesertProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 6),
            Abundance(ResourceIDMap.Seeds, 9),
            Abundance(ResourceIDMap.Fruit, 2),
            Abundance(ResourceIDMap.Roots, 8),
            Abundance(ResourceIDMap.Wood, 3),
            Abundance(ResourceIDMap.Grubs, 2),
            Abundance(ResourceIDMap.Fungus, 1)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 2),
            Abundance(ResourceIDMap.Seeds, 2),
            Abundance(ResourceIDMap.Fruit, 2),
            Abundance(ResourceIDMap.Roots, 1),
            Abundance(ResourceIDMap.Wood, 10),
            Abundance(ResourceIDMap.Grubs, 1),
            Abundance(ResourceIDMap.Fungus, 1)
        )
    };

    public static readonly BiomeVegetationProfile TundraProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 5),
            Abundance(ResourceIDMap.Seeds, 1),
            Abundance(ResourceIDMap.Fruit, 2),
            Abundance(ResourceIDMap.Roots, 3),
            Abundance(ResourceIDMap.Wood, 1),
            Abundance(ResourceIDMap.Grubs, 1),
            Abundance(ResourceIDMap.Fungus, 2)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 2),
            Abundance(ResourceIDMap.Seeds, 1),
            Abundance(ResourceIDMap.Fruit, 2),
            Abundance(ResourceIDMap.Roots, 1),
            Abundance(ResourceIDMap.Wood, 10),
            Abundance(ResourceIDMap.Grubs, 1),
            Abundance(ResourceIDMap.Fungus, 2)
        )
    };

    public static readonly BiomeVegetationProfile TaigaProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 6),
            Abundance(ResourceIDMap.Seeds, 2),
            Abundance(ResourceIDMap.Fruit, 3),
            Abundance(ResourceIDMap.Roots, 2),
            Abundance(ResourceIDMap.Wood, 2),
            Abundance(ResourceIDMap.Grubs, 4),
            Abundance(ResourceIDMap.Fungus, 6)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 2),
            Abundance(ResourceIDMap.Seeds, 2),
            Abundance(ResourceIDMap.Fruit, 1),
            Abundance(ResourceIDMap.Roots, 1),
            Abundance(ResourceIDMap.Wood, 10),
            Abundance(ResourceIDMap.Grubs, 1),
            Abundance(ResourceIDMap.Fungus, 2)
        )
    };

    public static readonly BiomeVegetationProfile TropicalProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 9),
            Abundance(ResourceIDMap.Seeds, 3),
            Abundance(ResourceIDMap.Fruit, 7),
            Abundance(ResourceIDMap.Roots, 4),
            Abundance(ResourceIDMap.Wood, 2),
            Abundance(ResourceIDMap.Grubs, 8),
            Abundance(ResourceIDMap.Fungus, 6)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 4),
            Abundance(ResourceIDMap.Seeds, 2),
            Abundance(ResourceIDMap.Fruit, 10),
            Abundance(ResourceIDMap.Roots, 1),
            Abundance(ResourceIDMap.Wood, 10),
            Abundance(ResourceIDMap.Grubs, 5),
            Abundance(ResourceIDMap.Fungus, 3)
        )
    };

    public static readonly BiomeVegetationProfile SavannaProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 10),
            Abundance(ResourceIDMap.Seeds, 5),
            Abundance(ResourceIDMap.Fruit, 1),
            Abundance(ResourceIDMap.Roots, 3),
            Abundance(ResourceIDMap.Wood, 1),
            Abundance(ResourceIDMap.Grubs, 3),
            Abundance(ResourceIDMap.Fungus, 1)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 2),
            Abundance(ResourceIDMap.Seeds, 1),
            Abundance(ResourceIDMap.Fruit, 2),
            Abundance(ResourceIDMap.Roots, 1),
            Abundance(ResourceIDMap.Wood, 10),
            Abundance(ResourceIDMap.Grubs, 1),
            Abundance(ResourceIDMap.Fungus, 1)
        )
    };

    public static readonly BiomeVegetationProfile TemperateProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 7),
            Abundance(ResourceIDMap.Seeds, 3),
            Abundance(ResourceIDMap.Fruit, 4),
            Abundance(ResourceIDMap.Roots, 3),
            Abundance(ResourceIDMap.Wood, 1),
            Abundance(ResourceIDMap.Grubs, 4),
            Abundance(ResourceIDMap.Fungus, 3)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 2),
            Abundance(ResourceIDMap.Seeds, 3),
            Abundance(ResourceIDMap.Fruit, 4),
            Abundance(ResourceIDMap.Roots, 1),
            Abundance(ResourceIDMap.Wood, 10),
            Abundance(ResourceIDMap.Grubs, 2),
            Abundance(ResourceIDMap.Fungus, 3)
        )
    };

    public static readonly BiomeVegetationProfile SteppeProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 9),
            Abundance(ResourceIDMap.Seeds, 6),
            Abundance(ResourceIDMap.Fruit, 1),
            Abundance(ResourceIDMap.Roots, 3),
            Abundance(ResourceIDMap.Wood, 1),
            Abundance(ResourceIDMap.Grubs, 2),
            Abundance(ResourceIDMap.Fungus, 1)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceIDMap.Greens, 1),
            Abundance(ResourceIDMap.Seeds, 2),
            Abundance(ResourceIDMap.Fruit, 1),
            Abundance(ResourceIDMap.Roots, 1),
            Abundance(ResourceIDMap.Wood, 10),
            Abundance(ResourceIDMap.Grubs, 1),
            Abundance(ResourceIDMap.Fungus, 1)
        )
    };

    public static readonly IReadOnlyDictionary<Biome, BiomeVegetationProfile>
        Profiles = new Dictionary<Biome, BiomeVegetationProfile>
        {
            [Biome.Desert] = DesertProfile,
            [Biome.Tundra] = TundraProfile,
            [Biome.Taiga] = TaigaProfile,
            [Biome.Savanna] = SavannaProfile,
            [Biome.Temperate] = TemperateProfile,
            [Biome.Steppe] = SteppeProfile,
            [Biome.Tropical] = TropicalProfile
        };

    private static ResourceAbundance Abundance(ResourceID resourceId, int relativeAbundance)
    {
        return new ResourceAbundance(resourceId, relativeAbundance);
    }
}

public sealed class BiomeVegetationProfile
{
    public VegetationAbundanceProfile LowVegetationProfile { get; set; }
    public VegetationAbundanceProfile HighVegetationProfile { get; set; }
}

public sealed class VegetationAbundanceProfile
{
    private readonly ResourceAbundance[] _abundances;

    public IReadOnlyList<ResourceAbundance> Abundances => _abundances;

    public VegetationAbundanceProfile(params ResourceAbundance[] abundances)
    {
        _abundances = abundances ?? throw new ArgumentNullException(nameof(abundances));
    }
}

public readonly struct ResourceAbundance
{
    public ResourceID ResourceId { get; }

    public int RelativeAbundance { get; }

    public ResourceAbundance(ResourceID resourceId, int relativeAbundance)
    {
        ResourceId = resourceId;
        RelativeAbundance = relativeAbundance;
    }
}