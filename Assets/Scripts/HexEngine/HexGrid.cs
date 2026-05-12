using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class HexGrid
{
    public HexData[] Grid { get; private set; }



    public (int min, int max) RowBounds;
    public (int min, int max) ColBounds;

    public int MiddleRow { get { return (RowBounds.max - RowBounds.min) / 2; } }

    public int MiddleCol { get { return (ColBounds.max - ColBounds.min / 2); } }

    public int Width { get { return (ColBounds.max - ColBounds.min); } }
    public int Height { get { return (RowBounds.max - RowBounds.min); } }

    public HexGrid(List<HexData> hexDataList)
    {
        Grid = new HexData[hexDataList.Count];

        SetBounds(hexDataList);

        foreach (HexData hexData in hexDataList)
        {
            Grid[GetArrayIndex(hexData.Coord)] = hexData;
        }
    }
    public bool TryGetHex(AxialCoordinate coord, out HexData hex)
    {
        hex = null;
        if (!CheckIsInBounds(coord)) return false;

        hex = Grid[GetArrayIndex(coord)];

        if (hex == null) return false;
        return true;
    }

    private int GetArrayIndex(AxialCoordinate coord)
    {
        var oddR = AxialGeometry.AxialToOddR(coord);
        return (oddR.row - RowBounds.min) * Width + (oddR.col - ColBounds.min);
    }

    public bool CheckIsInBounds(AxialCoordinate coord)
    {
        var oddR = AxialGeometry.AxialToOddR(coord);
        return oddR.row >= RowBounds.min && oddR.row <= RowBounds.max && oddR.col >= ColBounds.min && oddR.col <= ColBounds.max;
    }

    public List<HexData> GetHexData()
    { 
        return Grid.ToList();
    }

    public IEnumerable<HexData> GetValidHexes()
    {
        foreach (var data in Grid)
        {
            if (data != null) yield return data;
        }
    }

    public List<AxialCoordinate> GetAllAxialCoords()
    {
        List <AxialCoordinate> coordList = new List<AxialCoordinate>();

        foreach (HexData hexData in GetValidHexes())
        {
            coordList.Add(hexData.Coord);
        }

        return coordList;
    }

    private void SetBounds(List<HexData> hexDataList)
    {
        // Find odd-r bounds
        var first = AxialGeometry.AxialToOddR(hexDataList[0].Coord);
        RowBounds.min = first.row;
        RowBounds.max = first.row;
        ColBounds.min = first.col;
        ColBounds.max = first.col;

        foreach (HexData hexData in hexDataList)
        {
            var o = AxialGeometry.AxialToOddR(hexData.Coord);
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

    float GetLongitude01(AxialCoordinate coord)
    {
        float width = Mathf.Max(1f, ColBounds.max - ColBounds.min);
        return Mathf.Repeat((coord.Q - ColBounds.min) / width, 1f);
    }

    float GetLongitudeSigned(AxialCoordinate coord)
    {
        float halfWidth = Mathf.Max(1f, Mathf.Max(MiddleCol - ColBounds.min, ColBounds.max - MiddleCol));
        return Mathf.Clamp((coord.Q - MiddleCol) / halfWidth, -1f, 1f);
    }

    public Vector2 GetWindDirection(AxialCoordinate coord)
    {
        float lat = GetLatitudeSigned(coord);
        float lon = GetLongitudeSigned(coord);

        float a = 2f;
        float b = 2f;

        float dfdx = 0.707f * Mathf.Sin(a * Mathf.PI * lon);
        float dfdy = 0.707f * Mathf.Sin(b * Mathf.PI * lat);
        return CartesianGeometry.GetPerpendicularVector(new Vector2(dfdx, dfdy));
    }

    public int NumHexesFromSea(HexData data, int maxNumber, out HexData seaHex)
    {
        seaHex = null;
        if (data.ExtraData.IsSea) return 0;
        for (int n = 1; n < maxNumber; n++)
        {
            List<HexData> hexes = HexGridGeometry.HexesInRingOfRadiusOfHex(this, data, n);
            foreach (HexData hex in hexes)
            {
                if (hex.ExtraData.IsSea)
                {
                    seaHex = hex;
                    return n;
                }
            }
        }
        return maxNumber;
    }

    public int NumHexesFromFreshWater(HexData data)
    {
        int maxNumber = 8;
        for (int n = 1; n < maxNumber; n++)
        {
            List<HexData> hexes = HexGridGeometry.HexesInRingOfRadiusOfHex(this, data, n);
            foreach (HexData hex in hexes)
            {

            }
        }
        return maxNumber;
    }
}