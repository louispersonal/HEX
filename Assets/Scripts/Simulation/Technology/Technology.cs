using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Technology : IIndexedItem<TechnologyID>
{
    [SerializeField] private TechnologyID _id;
    public TechnologyID ID => _id;
}

[Serializable]
public struct TechnologyID : IEquatable<TechnologyID>
{
    public int Value;

    public TechnologyID(int value) => Value = value;
    
    public bool Equals(TechnologyID other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return obj is TechnologyID other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value;
    }
}