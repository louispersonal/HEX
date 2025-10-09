using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseHex
{
    // Both sizes are defined as inner radius of the hex
    public static float SceneSize = 1.15f; //1 unit in unity world space
    public static float InGameSize = 20f; //20km

    [SerializeField] private AxialCoordinate _coord;
    public AxialCoordinate Coord {  get { return _coord; } }

    public BaseHex(AxialCoordinate a)
    {
        _coord = a;
    }

    public BaseHex(int q, int r)
    {
        _coord = new AxialCoordinate(q, r);
    }
}
