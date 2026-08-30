using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridGeometry
{
    public static Vector2 AxialToScene(AxialCoordinate a)
    {
        return AxialGeometry.AxialToCartesian(a, HexView.SceneSize);
    }

    public static AxialCoordinate SceneToAxial(Vector2 p)
    {
        return AxialGeometry.CartesianToAxial(p, HexView.SceneSize);
    }

    public static (float q, float r) SceneToFractionalAxial(Vector3 p)
    {
        return AxialGeometry.CartesianToFractionalAxial(p, HexView.SceneSize);
    }

    public static bool TryGetHexAtScenePoint(HexGrid grid, Vector2 p, out Hex hex)
    {
        return grid.TryGetHex(SceneToAxial(p), out hex);
    }

    public static Vector2 GetRandomPointInHex(System.Random random, float buffer = 0)
    {
        float radius = HexView.InnerRadius - buffer;

        float angle = (float)random.NextDouble() * Mathf.PI * 2f;
        float distance = Mathf.Sqrt((float)random.NextDouble()) * radius;

        return new Vector2(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
    }
    
    public static List<Hex> GenerateHexShapedGrid(int N)
    {
        int hexCount = 1 + 3 * N * (N + 1);

        List<Hex> hexDataList = new List<Hex>(hexCount);

        for (int q = -N; q <= N; q++)
        {
            for (int r = Mathf.Max(-N, -q - N); r <= Mathf.Min(N, -q + N); r++)
            {
                Hex currentHex = new Hex(q, r);
                hexDataList.Add(currentHex);
            }
        }

        return hexDataList;
    }

    public static List<Hex> GenerateRectangularGrid(int columns, int rows)
    {
        List<Hex> hexDataList = new List<Hex>(columns * rows);

        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                Hex currentHex = new Hex(AxialGeometry.OddRToAxial((r, c)));
                hexDataList.Add(currentHex);
            }
        }

        return hexDataList;
    }

    public static float DistanceBetweenHexes(Hex a, Hex b)
    {
        return AxialGeometry.DistanceBetweenCoords(a.Coord, b.Coord);
    }

    public static List<Hex> HexesWithinRadiusOfHex(HexGrid grid, Hex a, int radius)
    {
        List<Hex> hexesInRange = new List<Hex>();

        List<AxialCoordinate> axials = AxialGeometry.CoordsWithinRadiusOfCoord(a.Coord, radius);

        foreach (AxialCoordinate axial in axials)
        {
            if (grid.TryGetHex(axial, out Hex neighborHex))
            {
                hexesInRange.Add(neighborHex);
            }
        }

        return hexesInRange;
    }

    public static List<Hex> HexesInRingOfRadiusOfHex(HexGrid grid, Hex a, int radius)
    {
        List<Hex> hexesInRange = new List<Hex>();

        List<AxialCoordinate> axials = AxialGeometry.CoordsInRingOfRadius(a.Coord, radius);

        foreach (AxialCoordinate axial in axials)
        {
            if (grid.TryGetHex(axial, out Hex neighborHex))
            {
                hexesInRange.Add(neighborHex);
            }
        }

        return hexesInRange;
    }
}
