using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culture
{
    public CultureID ID;
    public string Name;

    public CultureID? ParentID;
    public List<CultureID> Children;

    public int Depth;
    
    public Dictionary<ResourceID, float> GatheringProficiency { get; private set; }
}

public readonly struct CultureID : IEquatable<CultureID>
{
    public readonly int Value;
    public CultureID(int value) => Value = value;

    public Culture Culture => GameController.Instance.SessionManager.GameData.Cultures[this];

    public bool Equals(CultureID other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return obj is CultureID other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value;
    }
}