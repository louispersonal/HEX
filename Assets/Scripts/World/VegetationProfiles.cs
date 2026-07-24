using System;
using System.Collections.Generic;

public static class VegetationProfiles
{
    public static readonly BiomeVegetationProfile DesertProfile = new()
    {
        LowVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 1200f),
            DailyYield(ResourceIDMap.Seeds, 1800f),
            DailyYield(ResourceIDMap.Fruit, 400f),
            DailyYield(ResourceIDMap.Roots, 1600f),
            DailyYield(ResourceIDMap.Wood, 600f),
            DailyYield(ResourceIDMap.Grubs, 450f),
            DailyYield(ResourceIDMap.Fungus, 50f)
        ),

        HighVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 800f),
            DailyYield(ResourceIDMap.Seeds, 1000f),
            DailyYield(ResourceIDMap.Fruit, 900f),
            DailyYield(ResourceIDMap.Roots, 400f),
            DailyYield(ResourceIDMap.Wood, 5000f),
            DailyYield(ResourceIDMap.Grubs, 350f),
            DailyYield(ResourceIDMap.Fungus, 100f)
        )
    };

    public static readonly BiomeVegetationProfile TundraProfile = new()
    {
        LowVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 2600f),
            DailyYield(ResourceIDMap.Seeds, 550f),
            DailyYield(ResourceIDMap.Fruit, 1000f),
            DailyYield(ResourceIDMap.Roots, 1500f),
            DailyYield(ResourceIDMap.Wood, 350f),
            DailyYield(ResourceIDMap.Grubs, 550f),
            DailyYield(ResourceIDMap.Fungus, 1100f)
        ),

        HighVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 1100f),
            DailyYield(ResourceIDMap.Seeds, 400f),
            DailyYield(ResourceIDMap.Fruit, 900f),
            DailyYield(ResourceIDMap.Roots, 350f),
            DailyYield(ResourceIDMap.Wood, 6500f),
            DailyYield(ResourceIDMap.Grubs, 450f),
            DailyYield(ResourceIDMap.Fungus, 1000f)
        )
    };

    public static readonly BiomeVegetationProfile TaigaProfile = new()
    {
        LowVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 3000f),
            DailyYield(ResourceIDMap.Seeds, 1100f),
            DailyYield(ResourceIDMap.Fruit, 1700f),
            DailyYield(ResourceIDMap.Roots, 1200f),
            DailyYield(ResourceIDMap.Wood, 1000f),
            DailyYield(ResourceIDMap.Grubs, 1900f),
            DailyYield(ResourceIDMap.Fungus, 2800f)
        ),

        HighVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 1800f),
            DailyYield(ResourceIDMap.Seeds, 1800f),
            DailyYield(ResourceIDMap.Fruit, 1400f),
            DailyYield(ResourceIDMap.Roots, 600f),
            DailyYield(ResourceIDMap.Wood, 22000f),
            DailyYield(ResourceIDMap.Grubs, 1500f),
            DailyYield(ResourceIDMap.Fungus, 2600f)
        )
    };

    public static readonly BiomeVegetationProfile TropicalProfile = new()
    {
        LowVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 9000f),
            DailyYield(ResourceIDMap.Seeds, 2500f),
            DailyYield(ResourceIDMap.Fruit, 7000f),
            DailyYield(ResourceIDMap.Roots, 3500f),
            DailyYield(ResourceIDMap.Wood, 1800f),
            DailyYield(ResourceIDMap.Grubs, 7500f),
            DailyYield(ResourceIDMap.Fungus, 5500f)
        ),

        HighVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 5000f),
            DailyYield(ResourceIDMap.Seeds, 3000f),
            DailyYield(ResourceIDMap.Fruit, 13000f),
            DailyYield(ResourceIDMap.Roots, 1600f),
            DailyYield(ResourceIDMap.Wood, 35000f),
            DailyYield(ResourceIDMap.Grubs, 6500f),
            DailyYield(ResourceIDMap.Fungus, 4500f)
        )
    };

    public static readonly BiomeVegetationProfile SavannaProfile = new()
    {
        LowVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 9500f),
            DailyYield(ResourceIDMap.Seeds, 4800f),
            DailyYield(ResourceIDMap.Fruit, 1300f),
            DailyYield(ResourceIDMap.Roots, 3000f),
            DailyYield(ResourceIDMap.Wood, 800f),
            DailyYield(ResourceIDMap.Grubs, 2800f),
            DailyYield(ResourceIDMap.Fungus, 450f)
        ),

        HighVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 2300f),
            DailyYield(ResourceIDMap.Seeds, 1500f),
            DailyYield(ResourceIDMap.Fruit, 2800f),
            DailyYield(ResourceIDMap.Roots, 800f),
            DailyYield(ResourceIDMap.Wood, 15000f),
            DailyYield(ResourceIDMap.Grubs, 1400f),
            DailyYield(ResourceIDMap.Fungus, 700f)
        )
    };

    public static readonly BiomeVegetationProfile TemperateProfile = new()
    {
        LowVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 6500f),
            DailyYield(ResourceIDMap.Seeds, 3200f),
            DailyYield(ResourceIDMap.Fruit, 3600f),
            DailyYield(ResourceIDMap.Roots, 3000f),
            DailyYield(ResourceIDMap.Wood, 1200f),
            DailyYield(ResourceIDMap.Grubs, 3500f),
            DailyYield(ResourceIDMap.Fungus, 3300f)
        ),

        HighVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 2600f),
            DailyYield(ResourceIDMap.Seeds, 3600f),
            DailyYield(ResourceIDMap.Fruit, 4200f),
            DailyYield(ResourceIDMap.Roots, 900f),
            DailyYield(ResourceIDMap.Wood, 24000f),
            DailyYield(ResourceIDMap.Grubs, 2200f),
            DailyYield(ResourceIDMap.Fungus, 3000f)
        )
    };

    public static readonly BiomeVegetationProfile SteppeProfile = new()
    {
        LowVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 8500f),
            DailyYield(ResourceIDMap.Seeds, 5500f),
            DailyYield(ResourceIDMap.Fruit, 700f),
            DailyYield(ResourceIDMap.Roots, 3300f),
            DailyYield(ResourceIDMap.Wood, 450f),
            DailyYield(ResourceIDMap.Grubs, 1900f),
            DailyYield(ResourceIDMap.Fungus, 250f)
        ),

        HighVegetationProfile = new VegetationYieldProfile(
            DailyYield(ResourceIDMap.Greens, 1500f),
            DailyYield(ResourceIDMap.Seeds, 1700f),
            DailyYield(ResourceIDMap.Fruit, 1300f),
            DailyYield(ResourceIDMap.Roots, 650f),
            DailyYield(ResourceIDMap.Wood, 11000f),
            DailyYield(ResourceIDMap.Grubs, 900f),
            DailyYield(ResourceIDMap.Fungus, 450f)
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

    private static ResourceDailyYield DailyYield(ResourceID resourceId, float maximumDailyYield)
    {
        return new ResourceDailyYield(resourceId, maximumDailyYield);
    }
}

public sealed class BiomeVegetationProfile
{
    public VegetationYieldProfile LowVegetationProfile { get; set; }
    public VegetationYieldProfile HighVegetationProfile { get; set; }
}

public sealed class VegetationYieldProfile
{
    private readonly ResourceDailyYield[] _dailyYields;

    public IReadOnlyList<ResourceDailyYield> DailyYields => _dailyYields;

    public VegetationYieldProfile(params ResourceDailyYield[] dailyYields)
    {
        _dailyYields = dailyYields ?? throw new ArgumentNullException(nameof(dailyYields));
    }
}

public readonly struct ResourceDailyYield
{
    public ResourceID ResourceId { get; }
    
    public float MaximumDailyYield { get; }

    public ResourceDailyYield(ResourceID resourceId, float maximumDailyYield) 
    {

        ResourceId = resourceId;
        MaximumDailyYield = maximumDailyYield;
    }
}