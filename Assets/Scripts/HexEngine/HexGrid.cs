using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class HexGrid
{
    public Dictionary<AxialCoordinate, HexData> Grid { get; private set; }

    public bool TryGetHex(AxialCoordinate coord, out HexData hex)
    => Grid.TryGetValue(coord, out hex);

    public HexGrid(List<HexData> hexDataList)
    {
        Grid = new Dictionary<AxialCoordinate, HexData>();

        foreach (HexData hexData in hexDataList)
        {
            Grid.Add(hexData.Coord, hexData);
        }
    }

    public List<HexData> GetHexData()
    {
        return Grid.Values.ToList();
    }
}