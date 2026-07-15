using System.Collections.Generic;

public static class VegetationProfiles
{
    public static readonly BiomePlantProfile DesertProfile = new()
    {
        LowVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        },

        HighVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        }
    };

    public static readonly BiomePlantProfile TundraProfile = new()
    {
        LowVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        },

        HighVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        }
    };
    
    public static readonly BiomePlantProfile TaigaProfile = new()
    {
        LowVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        },

        HighVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        }
    };
    
    public static readonly BiomePlantProfile TropicalProfile = new()
    {
        LowVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        },

        HighVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        }
    };
    
    public static readonly BiomePlantProfile SavannaProfile = new()
    {
        LowVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        },

        HighVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        }
    };
    
    public static readonly BiomePlantProfile TemperateProfile = new()
    {
        LowVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        },

        HighVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        }
    };
    
    public static readonly BiomePlantProfile SteppeProfile = new()
    {
        LowVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        },

        HighVProfile = new PlantProfile
        {
            Roots = 0.40f,
            Seeds = 0.30f,
            Fruits = 0.15f,
            Nuts = 0.05f,
            Greens = 0.10f
        }
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
    public float Roots;
    public float Seeds;
    public float Fruits;
    public float Nuts;
    public float Greens;
}