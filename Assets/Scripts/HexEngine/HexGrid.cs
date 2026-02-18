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

    public (int min, int max) RowBounds;
    public (int min, int max) ColBounds;

    public int MiddleRow { get { return (RowBounds.max - RowBounds.min) / 2; } }

    public HexGrid(List<HexData> hexDataList)
    {
        Grid = new Dictionary<AxialCoordinate, HexData>();

        foreach (HexData hexData in hexDataList)
        {
            Grid.Add(hexData.Coord, hexData);
        }

        SetBounds(Grid.Keys.ToList());
    }

    public List<HexData> GetHexData()
    {
        return Grid.Values.ToList();
    }

    private void SetBounds(List<AxialCoordinate> axialCoords)
    {
        // Find odd-r bounds
        var first = AxialGeometry.AxialToOddR(axialCoords[0]);
        RowBounds.min = first.row;
        RowBounds.max = first.row;
        ColBounds.min = first.col;
        ColBounds.max = first.col;

        foreach (AxialCoordinate coord in axialCoords)
        {
            var o = AxialGeometry.AxialToOddR(coord);
            if (o.row < RowBounds.min) RowBounds.min = o.row;
            if (o.row > RowBounds.max) RowBounds.max = o.row;
            if (o.col < ColBounds.min) ColBounds.min = o.col;
            if (o.col > ColBounds.max) ColBounds.max = o.col;
        }
    }

    public float GetLatitude(AxialCoordinate coord)
    {
        return Mathf.Abs((float)(coord.R - MiddleRow) / (RowBounds.max - RowBounds.min));
    }
}