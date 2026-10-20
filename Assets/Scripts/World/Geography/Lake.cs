using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lake : ISpatialObject<LakeID>
{
    [SerializeField] public LakeID _id;
    public LakeID ID => _id;
    
    [SerializeField] public List<AxialCoordinate> _coords;
    public List<AxialCoordinate> Coords => _coords;

    public Lake(LakeID iD, List<AxialCoordinate> coords)
    {
        _id = iD;
        _coords = coords;
    }
}

[System.Serializable]
public struct LakeID
{
    public int Value;

    public LakeID(int value)
    {
        Value = value;
    }
}