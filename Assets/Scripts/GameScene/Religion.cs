using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Religion
{
    public ReligionID ID;
    public string Name;

    public ReligionID? ParentID;
    public List<ReligionID> Children;

    public int Depth;
}

public readonly struct ReligionID : IEquatable<ReligionID>
{
    public readonly int Value;
    public ReligionID(int value) => Value = value;

    public bool Equals(ReligionID other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return obj is ReligionID other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value;
    }
}