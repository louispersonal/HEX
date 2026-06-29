using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class AxialGeometry
{
    // NOTE axialSize is always considered as the outer radius of the hex (center to corner)
    public const float SQRT3 = 1.732050f;

    static readonly Vector2[] HexDirWorld =
    {
        new Vector2( 1f, 0f),                         // E
        new Vector2( 0.5f, -Mathf.Sqrt(3f)/2f),       // SE
        new Vector2(-0.5f, -Mathf.Sqrt(3f)/2f),       // SW
        new Vector2(-1f, 0f),                         // W
        new Vector2(-0.5f,  Mathf.Sqrt(3f)/2f),       // NW
        new Vector2( 0.5f,  Mathf.Sqrt(3f)/2f),       // NE
    };

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

    public static Vector2 AxialToCartesian(AxialCoordinate coord, float axialSize)
    {
        float x = axialSize * Mathf.Sqrt(3f) * (coord.Q + coord.R * 0.5f);
        float y = -axialSize * 1.5f * coord.R;

        return new Vector2(x, y);
    }

    public static AxialCoordinate ConvertVectorToAxialDirection(Vector2 vector)
    {
        if (vector.sqrMagnitude < 0.0001f)
            return AxialDirections.Directions[0];

        Vector2 dir = vector.normalized;

        int bestIndex = 0;
        float bestDot = float.NegativeInfinity;

        for (int i = 0; i < 6; i++)
        {
            float d = Vector2.Dot(dir, HexDirWorld[i]);
            if (d > bestDot)
            {
                bestDot = d;
                bestIndex = i;
            }
        }

        return AxialDirections.Directions[bestIndex];
    }


    public static (float q, float r) CartesianToFractionalAxial(Vector2 point, float axialSize)
    {
        float r = -point.y * (2f / (3f * axialSize));
        float q = (point.x / (Mathf.Sqrt(3f) * axialSize)) + (point.y / (3f * axialSize));

        return (q, r);
    }

    public static AxialCoordinate FractionalAxialToAxial((float q, float r) fractionalAxial)
    {
        return new AxialCoordinate((int)Mathf.Round(fractionalAxial.q), (int)Mathf.Round(fractionalAxial.r));
    }

    public static AxialCoordinate CartesianToAxial(Vector2 point, float axialSize)
    {
        return FractionalAxialToAxial(CartesianToFractionalAxial(point, axialSize));
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

        AxialCoordinate coord = a + (dirs[(int)AxialCardinalDirections.NW] * radius);
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

    public static Dictionary<AxialCoordinate, Vector2> ConvertAxialSetToBoundedCartesian(List<AxialCoordinate> axialCoords, Vector2 bottomLeftBound, Vector2 topRightBound, out float adjustedHexSize, out Vector2 usedTopRightBound)
    {
        if (axialCoords == null || axialCoords.Count == 0)
        {
            adjustedHexSize = 0f;
            usedTopRightBound = Vector2.zero;
            return new Dictionary<AxialCoordinate, Vector2>();
        }

        // Find odd-r bounds
        var first = AxialToOddR(axialCoords[0]);
        int minRow = first.row, maxRow = first.row;
        int minCol = first.col, maxCol = first.col;

        foreach (AxialCoordinate coord in axialCoords)
        {
            var o = AxialToOddR(coord);
            if (o.row < minRow) minRow = o.row;
            if (o.row > maxRow) maxRow = o.row;
            if (o.col < minCol) minCol = o.col;
            if (o.col > maxCol) maxCol = o.col;
        }

        // Unit-space (hexSize=1) corner centers in cartesian
        Vector2 CornerCenter_Unit(int col, int row)
        {
            // pointy-top odd-r -> cartesian (size=1), consistent with AxialToCartesian's orientation
            // IMPORTANT: match your AxialToCartesian sign conventions.
            float x = Mathf.Sqrt(3f) * (col + ((row & 1) == 1 ? 0.5f : 0f));
            float y = -1.5f * row; // negative so increasing row goes "down" like your axial uses negative y
            return new Vector2(x, y);
        }

        Vector2 c00 = CornerCenter_Unit(minCol, minRow);
        Vector2 c01 = CornerCenter_Unit(minCol, maxRow);
        Vector2 c10 = CornerCenter_Unit(maxCol, minRow);
        Vector2 c11 = CornerCenter_Unit(maxCol, maxRow);

        float minX = Mathf.Min(c00.x, c01.x, c10.x, c11.x);
        float maxX = Mathf.Max(c00.x, c01.x, c10.x, c11.x);
        float minY = Mathf.Min(c00.y, c01.y, c10.y, c11.y);
        float maxY = Mathf.Max(c00.y, c01.y, c10.y, c11.y);

        float dxCenters = maxX - minX;
        float dyCenters = maxY - minY;

        // Add hex extents (size=1): width=?3, height=2
        float totalHorizontalSpan = dxCenters + Mathf.Sqrt(3f);
        float totalVerticalSpan = dyCenters + 2f;

        float availableW = topRightBound.x - bottomLeftBound.x;
        float availableH = topRightBound.y - bottomLeftBound.y;

        float sizeFromHorizontal = availableW / totalHorizontalSpan;
        float sizeFromVertical = availableH / totalVerticalSpan;

        adjustedHexSize = Mathf.Min(sizeFromHorizontal, sizeFromVertical);
        usedTopRightBound = bottomLeftBound + new Vector2(
            totalHorizontalSpan * adjustedHexSize,
            totalVerticalSpan * adjustedHexSize );
        
        // Now compute offset so the grid's min tile-edge maps to bottomLeftBound
        float halfW = Mathf.Sqrt(3f) * 0.5f * adjustedHexSize;
        float halfH = 1f * adjustedHexSize;

        Vector2 minCenterWorld = new Vector2(minX * adjustedHexSize, minY * adjustedHexSize);
        Vector2 minTileEdgeWorld = new Vector2(minCenterWorld.x - halfW, minCenterWorld.y - halfH);

        Vector2 offset = bottomLeftBound - minTileEdgeWorld;

        var result = new Dictionary<AxialCoordinate, Vector2>(axialCoords.Count);
        foreach (AxialCoordinate aC in axialCoords)
            result[aC] = AxialToCartesian(aC, adjustedHexSize) + offset;

        return result;
    }
}
