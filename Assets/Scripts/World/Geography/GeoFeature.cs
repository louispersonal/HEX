using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GeoFeature : ISpatialObject<GeoID>
{
    [SerializeField] private GeoID _id;
    public GeoID ID => _id;
    
    [SerializeField] private List<AxialCoordinate> _coords;
    public List<AxialCoordinate> Coords => _coords;
    public GeoFeatureType Type;

    public GeoFeature(GeoID iD, List<AxialCoordinate> coords, GeoFeatureType type)
    {
        _id = iD;
        _coords = coords;
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
    Volcano,
    CaveSystem
}