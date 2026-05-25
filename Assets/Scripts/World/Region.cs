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

    private int _hasRiver = -1;

    private int _isCoastal = -1;

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

    public List<HexData> GetHexesInRegion(WorldData world)
    {
        List<HexData> result = new();
        Stack<HexData> stack = new();

        world.Grid.TryGetHex(SeedCoord, out HexData seedHex);
        stack.Push(seedHex);

        while (stack.Count > 0)
        {
            HexData hex = stack.Pop();

            if (hex == null) continue;
            if (hex.ExtraData.RegionId != ID) continue;

            result.Add(hex);

            foreach (HexData neighbor in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, hex, 1))
            {
                if (neighbor != null && neighbor.ExtraData.RegionId == ID && !result.Contains(neighbor))
                {
                    stack.Push(neighbor);
                }
            }
        }

        return result;
    }
}
