using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseHex
{
    // Both sizes are defined as inner radius of the hex
    public static float SceneSize = 1f; //1 unit in unity world space
    public static float InGameSize = 20f; //20km

    public AxialCoordinate Coord { get; }

    public BaseHex(int q, int r)
    {
        Coord = new AxialCoordinate(q, r);
    }

    public List<BaseHex> GetNeighbors()
    {
        List<BaseHex> neighbors = new List<BaseHex>();

        foreach (AxialCoordinate dir in AxialDirections.Directions)
        {
            AxialCoordinate neighborCoord = Coord + dir;
            if (BaseHexGrid.Instance.TryGetHex(neighborCoord, out BaseHex neighborHex))
            {
                neighbors.Add(neighborHex);
            }
        }

        return neighbors;
    }

    public float DistanceToHex(BaseHex target)
    {
        return BaseHexGrid.Instance.DistanceBetweenHexes(this, target);
    }

    public List<BaseHex> HexesWithinRadius(int radius)
    {
        return BaseHexGrid.Instance.HexesWithinRadiusOfHex(this, radius);
    }

    public Vector2 GetScenePosition()
    {
        return BaseHexGrid.Instance.AxialToSceneConversion(Coord);
    }
}
