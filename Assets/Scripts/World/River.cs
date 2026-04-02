using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River
{
    public RiverID ID;
    public List<AxialCoordinate> Hexes;
    public AxialCoordinate Source;
    public AxialCoordinate Mouth;

    public River (RiverID iD, AxialCoordinate source)
    {
        Source = source;
        Hexes = new List<AxialCoordinate>();
        Hexes.Add(source);
        ID = iD;
    }
}

public struct RiverID
{
    public int Value;

    public RiverID(int value)
    {
        Value = value;
    }
}