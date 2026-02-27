using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River
{
    public RiverID ID;
    public List<AxialCoordinate> Tiles;
    public AxialCoordinate Source;
    public AxialCoordinate Mouth;
}

public struct RiverID
{
    public int Value;

    public RiverID(int value)
    {
        Value = value;
    }
}