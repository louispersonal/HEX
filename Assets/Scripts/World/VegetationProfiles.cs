using System;
using System.Collections.Generic;

public static class VegetationProfiles
{
    public static readonly BiomeVegetationProfile DesertProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 6),
            Abundance(ResourceRefs.Seeds, 9),
            Abundance(ResourceRefs.Fruit, 2),
            Abundance(ResourceRefs.Roots, 8),
            Abundance(ResourceRefs.Wood, 3),
            Abundance(ResourceRefs.Grubs, 2),
            Abundance(ResourceRefs.Fungus, 1)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 2),
            Abundance(ResourceRefs.Seeds, 2),
            Abundance(ResourceRefs.Fruit, 2),
            Abundance(ResourceRefs.Roots, 1),
            Abundance(ResourceRefs.Wood, 10),
            Abundance(ResourceRefs.Grubs, 1),
            Abundance(ResourceRefs.Fungus, 1)
        )
    };

    public static readonly BiomeVegetationProfile TundraProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 5),
            Abundance(ResourceRefs.Seeds, 1),
            Abundance(ResourceRefs.Fruit, 2),
            Abundance(ResourceRefs.Roots, 3),
            Abundance(ResourceRefs.Wood, 1),
            Abundance(ResourceRefs.Grubs, 1),
            Abundance(ResourceRefs.Fungus, 2)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 2),
            Abundance(ResourceRefs.Seeds, 1),
            Abundance(ResourceRefs.Fruit, 2),
            Abundance(ResourceRefs.Roots, 1),
            Abundance(ResourceRefs.Wood, 10),
            Abundance(ResourceRefs.Grubs, 1),
            Abundance(ResourceRefs.Fungus, 2)
        )
    };

    public static readonly BiomeVegetationProfile TaigaProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 6),
            Abundance(ResourceRefs.Seeds, 2),
            Abundance(ResourceRefs.Fruit, 3),
            Abundance(ResourceRefs.Roots, 2),
            Abundance(ResourceRefs.Wood, 2),
            Abundance(ResourceRefs.Grubs, 4),
            Abundance(ResourceRefs.Fungus, 6)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 2),
            Abundance(ResourceRefs.Seeds, 2),
            Abundance(ResourceRefs.Fruit, 1),
            Abundance(ResourceRefs.Roots, 1),
            Abundance(ResourceRefs.Wood, 10),
            Abundance(ResourceRefs.Grubs, 1),
            Abundance(ResourceRefs.Fungus, 2)
        )
    };

    public static readonly BiomeVegetationProfile TropicalProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 9),
            Abundance(ResourceRefs.Seeds, 3),
            Abundance(ResourceRefs.Fruit, 7),
            Abundance(ResourceRefs.Roots, 4),
            Abundance(ResourceRefs.Wood, 2),
            Abundance(ResourceRefs.Grubs, 8),
            Abundance(ResourceRefs.Fungus, 6)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 4),
            Abundance(ResourceRefs.Seeds, 2),
            Abundance(ResourceRefs.Fruit, 10),
            Abundance(ResourceRefs.Roots, 1),
            Abundance(ResourceRefs.Wood, 10),
            Abundance(ResourceRefs.Grubs, 5),
            Abundance(ResourceRefs.Fungus, 3)
        )
    };

    public static readonly BiomeVegetationProfile SavannaProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 10),
            Abundance(ResourceRefs.Seeds, 5),
            Abundance(ResourceRefs.Fruit, 1),
            Abundance(ResourceRefs.Roots, 3),
            Abundance(ResourceRefs.Wood, 1),
            Abundance(ResourceRefs.Grubs, 3),
            Abundance(ResourceRefs.Fungus, 1)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 2),
            Abundance(ResourceRefs.Seeds, 1),
            Abundance(ResourceRefs.Fruit, 2),
            Abundance(ResourceRefs.Roots, 1),
            Abundance(ResourceRefs.Wood, 10),
            Abundance(ResourceRefs.Grubs, 1),
            Abundance(ResourceRefs.Fungus, 1)
        )
    };

    public static readonly BiomeVegetationProfile TemperateProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 7),
            Abundance(ResourceRefs.Seeds, 3),
            Abundance(ResourceRefs.Fruit, 4),
            Abundance(ResourceRefs.Roots, 3),
            Abundance(ResourceRefs.Wood, 1),
            Abundance(ResourceRefs.Grubs, 4),
            Abundance(ResourceRefs.Fungus, 3)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 2),
            Abundance(ResourceRefs.Seeds, 3),
            Abundance(ResourceRefs.Fruit, 4),
            Abundance(ResourceRefs.Roots, 1),
            Abundance(ResourceRefs.Wood, 10),
            Abundance(ResourceRefs.Grubs, 2),
            Abundance(ResourceRefs.Fungus, 3)
        )
    };

    public static readonly BiomeVegetationProfile SteppeProfile = new()
    {
        LowVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 9),
            Abundance(ResourceRefs.Seeds, 6),
            Abundance(ResourceRefs.Fruit, 1),
            Abundance(ResourceRefs.Roots, 3),
            Abundance(ResourceRefs.Wood, 1),
            Abundance(ResourceRefs.Grubs, 2),
            Abundance(ResourceRefs.Fungus, 1)
        ),

        HighVegetationProfile = new VegetationAbundanceProfile(
            Abundance(ResourceRefs.Greens, 1),
            Abundance(ResourceRefs.Seeds, 2),
            Abundance(ResourceRefs.Fruit, 1),
            Abundance(ResourceRefs.Roots, 1),
            Abundance(ResourceRefs.Wood, 10),
            Abundance(ResourceRefs.Grubs, 1),
            Abundance(ResourceRefs.Fungus, 1)
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