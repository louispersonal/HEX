using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Region
{
    public ushort ID;

    public int Size;

    public float TotalLowVegetation;

    public float TotalHighVegetation;

    public AxialCoordinate SeedCoord;
    
    public Dictionary<SpeciesID, int> Animals = new();

    public Biome Biome;

    private int _hasRiver = -1;

    private int _isCoastal = -1;

    private int _riverLength = -1;

    public Region(ushort iD, AxialCoordinate seedCoord)
    {
        ID = iD;
        SeedCoord = seedCoord;
    }

    public bool HasRiver(WorldData world)
    {
        if (_hasRiver == -1)
        {
            _hasRiver = 0;
            var hexes = GetHexesInRegion(world);
            foreach (var hex in hexes)
            {
                if (world.Rivers.ContainsAt(hex.Coord))
                {
                    _hasRiver = 1;
                    break;
                }
            }
        }

        return _hasRiver == 1;
    }

    public bool IsCoastal(WorldData world)
    {
        if (_isCoastal == -1)
        {
            _isCoastal = 0;
            var hexes = GetHexesInRegion(world);
            foreach (var hex in hexes)
            {
                foreach (var neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, hex, 1))
                {
                    if (neighbor.ExtraData.IsSea)
                    {
                        _isCoastal = 1;
                        break;
                    }
                }
                if (_isCoastal == 1) break;
            }
        }

        return _isCoastal == 1;
    }

    public int RiverLength(WorldData world)
    {
        if (_riverLength == -1)
        {
            _riverLength = 0;
            var hexes = GetHexesInRegion(world);
            foreach (var hex in hexes)
            {
                if (world.Rivers.ContainsAt(hex.Coord))
                {
                    _riverLength++;
                }
            }
        }

        return _riverLength;
    }

    public List<Hex> GetHexesInRegion(WorldData world)
    {
        List<Hex> result = new();
        Stack<Hex> stack = new();

        world.Grid.TryGetHex(SeedCoord, out Hex seedHex);
        stack.Push(seedHex);
        
        while (stack.Count > 0)
        {
            Hex hex = stack.Pop();

            if (hex == null) continue;
            if (hex.ExtraData.RegionId != ID) continue;

            result.Add(hex);

            foreach (Hex neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, hex, 1))
            {
                if (neighbor != null && neighbor.ExtraData.RegionId == ID && !result.Contains(neighbor))
                {
                    stack.Push(neighbor);
                }
            }
        }

        return result;
    }
    
    public ResourceCollection PreviewAvailableResources(HexGrid grid)
    {
        Hex seedHex = grid.GetHex(SeedCoord);
        
        var highVegAbundances = VegetationProfiles.Profiles[seedHex.ExtraData.Biome]
            .HighVegetationProfile.Abundances;

        float highVegAbundanceConversionFactor = 100 * TotalHighVegetation;

        var availableContents = new ResourceCollection();
        foreach (var abundance in highVegAbundances)
        {
            availableContents.Deposit(abundance.ResourceId,
                abundance.RelativeAbundance * highVegAbundanceConversionFactor);
        }

        var lowVegAbundances = VegetationProfiles.Profiles[seedHex.ExtraData.Biome]
            .LowVegetationProfile.Abundances;

        float lowVegAbundanceConversionFactor = 100 * TotalLowVegetation;

        foreach (var abundance in lowVegAbundances)
        {
            availableContents.Deposit(abundance.ResourceId,
                abundance.RelativeAbundance * lowVegAbundanceConversionFactor);
        }
        
        return availableContents;
    }
}
