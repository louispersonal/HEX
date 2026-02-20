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

    public float GetLatitude01(AxialCoordinate coord)
    {
        float halfHeight = Mathf.Max(1f, Mathf.Max(MiddleRow - RowBounds.min, RowBounds.max - MiddleRow));
        return Mathf.Clamp01(Mathf.Abs(coord.R - MiddleRow) / halfHeight);
    }

    public float GetLatitudeSigned(AxialCoordinate coord)
    {
        float halfHeight = Mathf.Max(1f, Mathf.Max(MiddleRow - RowBounds.min, RowBounds.max - MiddleRow));
        return Mathf.Clamp((coord.R - MiddleRow) / halfHeight, -1f, 1f);
    }

    float GetLongitude01(AxialCoordinate c)
    {
        float width = Mathf.Max(1f, ColBounds.max - ColBounds.min);
        return Mathf.Repeat((c.Q - ColBounds.min) / width, 1f);
    }

    public Vector2 GetWindDirection(AxialCoordinate coord)
    {
        float lat = GetLatitude01(coord);
        float lon = GetLongitude01(coord);

        float phaseAmp = 0.35f;
        float phaseFreq = 2f;

        float theta = 2f * Mathf.PI * lat
                    + phaseAmp * Mathf.Sin(2f * Mathf.PI * phaseFreq * lon);

        return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
    }
}