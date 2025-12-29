using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridGeometry
{
    public static Vector2 AxialToScene(AxialCoordinate a)
    {
        float x = HexView.SceneSize * Mathf.Sqrt(3f) * (a.Q + (a.R / 2f));
        float y = HexView.SceneSize * (1.5f) * a.R;
        return new Vector2(x, y);
    }

    public static AxialCoordinate SceneToAxial(Vector2 p)
    {
        float r = p.y * (2f / (3f * HexView.SceneSize));
        float q = (p.x / (Mathf.Sqrt(3f) * HexView.SceneSize)) - (p.y / (3f * HexView.SceneSize));

        return new AxialCoordinate((int)Mathf.Round(q), (int)Mathf.Round(r));
    }

    public static (float q, float r) SceneToFractionalAxial(Vector3 p)
    {
        float r = p.y * (2f / (3f * HexView.SceneSize));
        float q = (p.x / (Mathf.Sqrt(3f) * HexView.SceneSize)) - (p.y / (3f * HexView.SceneSize));

        return (q, r);
    }

    public HexData GetHexAtScenePoint(HexGrid grid, Vector2 p)
    {
        if (grid.TryGetHex(SceneToAxial(p), out HexData hex))
        {
            return hex;
        }
        Debug.Log("No hex at point: " + p.ToString());
        return null;
    }

    public HexGrid GenerateHexShapedGrid(int N)
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

        return new HexGrid(hexDataList);
    }

    public HexGrid GenerateRectangularGrid(int columns, int rows)
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

        return new HexGrid(hexDataList);
    }

    public float DistanceBetweenHexes(HexData a, HexData b)
    {
        return AxialGeometry.DistanceBetweenCoords(a.Coord, b.Coord);
    }

    public List<HexData> HexesWithinRadiusOfHex(HexGrid grid, HexData a, int radius)
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

    public List<HexData> HexesInRingOfRadiusOfHex(HexGrid grid, HexData a, int radius)
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
