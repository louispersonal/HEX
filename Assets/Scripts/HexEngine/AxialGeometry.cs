using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxialGeometry
{
    public static (int row, int col) AxialToOddR(AxialCoordinate coord)
    {
        int parity = coord.R & 1;
        int col = coord.Q + (coord.R - parity) / 2;
        return (coord.R, col);
    }

    public static (int row, int col) AxialToOddR(int q, int r)
    {
        int parity = r & 1;
        int col = q + (r - parity) / 2;
        return (r, col);
    }

    public static AxialCoordinate OddRToAxial((int row, int col) o)
    {
        int parity = o.row & 1;
        int q = o.col - (o.row - parity) / 2;
        return new AxialCoordinate(q, o.row);
    }

    public static Vector2 AxialToRelativeCartesian(AxialCoordinate coord, Vector2 origin, float axialSize)
    {
        float x = axialSize * Mathf.Sqrt(3f) * (coord.Q + coord.R * 0.5f);
        float y = -axialSize * 1.5f * coord.R;

        return origin + new Vector2(x, y);
    }

    public static float DistanceBetweenCoords(AxialCoordinate a, AxialCoordinate b)
    {
        AxialCoordinate diff = a - b;
        return (Mathf.Abs(diff.Q) + Mathf.Abs(diff.R) + Mathf.Abs(diff.Q + diff.R)) / 2;
    }

    public static List<AxialCoordinate> CoordsWithinRadiusOfCoord(AxialCoordinate a, int radius)
    {
        int count = 1 + 3 * radius * (radius + 1);

        List<AxialCoordinate> coordsInRange = new List<AxialCoordinate>(count);

        for (int q = -radius; q <= radius; q++)
        {
            for (int r = Mathf.Max(-radius, -q - radius); r <= Mathf.Min(radius, -q + radius); r++)
            {
                AxialCoordinate hexCoord = a + new AxialCoordinate(q, r);
                coordsInRange.Add(hexCoord);
            }
        }

        return coordsInRange;
    }

    public static List<AxialCoordinate> CoordsInRingOfRadius(AxialCoordinate a, int radius)
    {
        List<AxialCoordinate> outList = new List<AxialCoordinate>();
        if (radius == 0) { outList.Add(a); return outList; }

        // axial directions (pointy-top)
        AxialCoordinate[] dirs = AxialDirections.Directions;

        AxialCoordinate coord = a + (dirs[4] * radius);
        for (int i = 0; i < 6; i++)
        {
            for (int step = 0; step < radius; step++)
            {
                outList.Add(coord);
                coord += dirs[i];
            }
        }

        return outList;
    }

    public static List<AxialCoordinate> CoordsInRingsOfRadii(AxialCoordinate a, int minRadius, int maxRadius)
    {
        List<AxialCoordinate> outList = new List<AxialCoordinate>();

        for (int r = minRadius; r <= maxRadius; r++)
        {
            outList.AddRange(CoordsInRingOfRadius(a, r));
        }

        return outList;
    }
}
