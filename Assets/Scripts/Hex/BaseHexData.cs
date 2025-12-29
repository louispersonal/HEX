using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseHexData
{
    // Both sizes are defined as inner radius of the hex
    public static float InGameSize = 20f; //20km

    [SerializeField] private AxialCoordinate _coord;
    public AxialCoordinate Coord {  get { return _coord; } }

    public BaseHexData(AxialCoordinate a)
    {
        _coord = a;
    }

    public BaseHexData(int q, int r)
    {
        _coord = new AxialCoordinate(q, r);
    }
}
