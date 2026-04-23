using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public ushort ID;

    public int Size;

    public AxialCoordinate SeedCoord;

    public Region(ushort iD, AxialCoordinate seedCoord)
    {
        ID = iD;
        SeedCoord = seedCoord;
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
