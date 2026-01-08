using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridGeometry
{
    public static Vector2 AxialToScene(AxialCoordinate a)
    {
        return AxialGeometry.AxialToRelativeCartesian(a, Vector2.zero, HexView.SceneSize);
    }

    public static AxialCoordinate SceneToAxial(Vector2 p)
    {
        return AxialGeometry.RelativeCartesianToAxial(p, Vector2.zero, HexView.SceneSize);
    }

    public static (float q, float r) SceneToFractionalAxial(Vector3 p)
    {
        return AxialGeometry.RelativeCartesianToFractionalAxial(p, Vector2.zero, HexView.SceneSize);
    }

    public static HexData GetHexAtScenePoint(HexGrid grid, Vector2 p)
    {
        if (grid.TryGetHex(SceneToAxial(p), out HexData hex))
        {
            return hex;
        }
        Debug.Log("No hex at point: " + p.ToString());
        return null;
    }

    public static List<HexData> GenerateHexShapedGrid(int N)
    {
        int hexCount = 1 + 3 * N * (N + 1);

        List<HexData> hexDataList = new List<HexData>(hexCount);

        for (int q = -N; q <= N; q++)
        {
            for (int r = Mathf.Max(-N, -q - N); r <= Mathf.Min(N, -q + N); r++)
            {
                HexData currentHex = new HexData(q, r);
                hexDataList.Add(currentHex);
            }
        }

        return hexDataList;
    }

    public static List<HexData> GenerateRectangularGrid(int columns, int rows)
    {
        List<HexData> hexDataList = new List<HexData>(columns * rows);

        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                HexData currentHex = new HexData(AxialGeometry.OddRToAxial((r, c)));
                hexDataList.Add(currentHex);
            }
        }

        return hexDataList;
    }

    public static float DistanceBetweenHexes(HexData a, HexData b)
    {
        return AxialGeometry.DistanceBetweenCoords(a.Coord, b.Coord);
    }

    public static List<HexData> HexesWithinRadiusOfHex(HexGrid grid, HexData a, int radius)
    {
        List<HexData> hexesInRange = new List<HexData>();

        List<AxialCoordinate> axials = AxialGeometry.CoordsWithinRadiusOfCoord(a.Coord, radius);

        foreach (AxialCoordinate axial in axials)
        {
            if (grid.TryGetHex(axial, out HexData neighborHex))
            {
                hexesInRange.Add(neighborHex);
            }
        }

        return hexesInRange;
    }

    public static List<HexData> HexesInRingOfRadiusOfHex(HexGrid grid, HexData a, int radius)
    {
        List<HexData> hexesInRange = new List<HexData>();

        List<AxialCoordinate> axials = AxialGeometry.CoordsInRingOfRadius(a.Coord, radius);

        foreach (AxialCoordinate axial in axials)
        {
            if (grid.TryGetHex(axial, out HexData neighborHex))
            {
                hexesInRange.Add(neighborHex);
            }
        }

        return hexesInRange;
    }
}
