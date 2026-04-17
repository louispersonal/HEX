using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class CartesianGeometry
{
    public static List<Vector2Int> BresenhamsLine(Vector2Int start, Vector2Int end, int width=1)
    {
        List<Vector2Int> points = new List<Vector2Int>();

        int x0 = start.x;
        int y0 = start.y;
        int x1 = end.x;
        int y1 = end.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);

        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;

        int err = dx - dy;

        while (true)
        {
            points.AddRange(GetCircle(new Vector2Int(x0, y0), width).Except(points));

            if (x0 == x1 && y0 == y1) break;

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        return points;
    }

    public static Vector2 GetPerpendicularVector(Vector2 vec, bool clockwise=true)
    {
        if (clockwise)
        {
            return new Vector2(-vec.y, vec.x);
        }
        else
        {
            return new Vector2(vec.y, -vec.x);
        }
    }

    public static List<Vector2Int> GetCircle(Vector2Int center, int radius)
    {
        List<Vector2Int> points = new List<Vector2Int>();
        // create bounding box and then check if points satisfy (x-cx)^2 + (y-cy)^2 = r
        for (int y = center.y - radius; y <= center.y + radius; y++)
        {
            for (int x = center.x - radius; x <= center.x + radius; x++)
            {
                if (Mathf.Pow(x - center.x, 2) + Mathf.Pow(y - center.y, 2) <= radius)
                {
                    points.Add(new Vector2Int(x, y));
                }
            }
        }
        return points;
    }
}
