using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GeoFeature
{
    public GeoID ID;
    public List<AxialCoordinate> Coords;
    public GeoFeatureType Type;

    public GeoFeature(GeoID iD, List<AxialCoordinate> coords, GeoFeatureType type)
    {
        ID = iD;
        Coords = coords;
        Type = type;
    }
}

[System.Serializable]
public struct GeoID
{
    public int Value;

    public GeoID(int value)
    {
        Value = value;
    }
}

public enum GeoFeatureType
{
    Canyon,
    Mountain,
    NaturalSpring,
    RockFormation,
    Waterfall,
    Valley,
    Cliff,
    Volcano
}