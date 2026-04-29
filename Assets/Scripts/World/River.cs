using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class River
{
    public RiverID ID;
    public List<AxialCoordinate> Coords;
    public AxialCoordinate Source;
    public AxialCoordinate Mouth;

    public River (RiverID iD, AxialCoordinate source)
    {
        Source = source;
        Coords = new List<AxialCoordinate>();
        Coords.Add(source);
        ID = iD;
    }
}

[System.Serializable]
public struct RiverID
{
    public int Value;

    public RiverID(int value)
    {
        Value = value;
    }
}