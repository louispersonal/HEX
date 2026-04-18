using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake
{
    public LakeID ID;
    public List<AxialCoordinate> Coords;

    public Lake(LakeID iD, List<AxialCoordinate> coords)
    {
        ID = iD;
        Coords = coords;
    }
}


public struct LakeID
{
    public int Value;

    public LakeID(int value)
    {
        Value = value;
    }
}