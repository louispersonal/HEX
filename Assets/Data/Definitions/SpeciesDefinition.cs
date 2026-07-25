using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpeciesDefinition : IDatabaseItem<SpeciesID>
{
    [SerializeField] private SpeciesID _id;
    
    public SpeciesID Id => _id;
    
    public AnimalArchetypeID ArchetypeId;
    public string SpeciesName;
    public List<AnimalTags> Tags;
}

[Serializable]
public readonly struct SpeciesID : IEquatable<SpeciesID>
{
    public readonly ushort Value;

    public SpeciesID(ushort value)
    {
        Value = value;
    }

    public bool Equals(SpeciesID other) => Value == other.Value;

    public override bool Equals(object obj) => obj is SpeciesID other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(SpeciesID left, SpeciesID right) => left.Equals(right);

    public static bool operator !=(SpeciesID left, SpeciesID right) => !left.Equals(right);
}