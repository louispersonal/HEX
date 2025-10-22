using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxialConverter
{
    public static Vector2 AxialToCartesianConversion(AxialCoordinate a, float hexSize)
    {
        float x = hexSize * Mathf.Sqrt(3f) * (a.Q + (a.R / 2f));
        float y = hexSize * (1.5f) * a.R;
        return new Vector2(x, y);
    }

    public static AxialCoordinate CartesianToAxialConversion(Vector2 p, float hexSize)
    {
        float r = p.y * (2f / (3f * hexSize));
        float q = (p.x / (Mathf.Sqrt(3f) * hexSize)) - (p.y / (3f * hexSize));

        return new AxialCoordinate((int)Mathf.Round(q), (int)Mathf.Round(r));
    }
}
