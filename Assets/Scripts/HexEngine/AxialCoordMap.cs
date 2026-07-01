using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxialCoordMap
{
    public Dictionary<AxialCoordinate, Vector2> Map { get; private set; }
    public Vector2 BottomLeftBound { get; private set; }
    public Vector2 TopRightBound { get; private set; }
    public float HexSize { get; private set; }

    public AxialCoordMap(Dictionary<AxialCoordinate, Vector2> map, Vector2 bottomLeftBound, Vector2 topRightBound,
        float hexSize)
    {
        Map = map;
        BottomLeftBound = bottomLeftBound;
        TopRightBound = topRightBound;
        HexSize = hexSize;
    }
}
