using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AnimalArchetypeDefinition : IDatabaseItem<AnimalArchetypeID>
{
    [SerializeField] private AnimalArchetypeID _id;
    
    public AnimalArchetypeID Id => _id;
    
    public string CommonName;

    public SizeTier Size;

    public Resources[] Eats;

    public float NutritionRequirement
    {
        get
        {
            switch (Size)
            {
                case SizeTier.Small:
                    return 1;
                case SizeTier.Medium:
                    return 10;
                default:
                    return 100;
            }
        }
    }
}

[Serializable]
public readonly struct AnimalArchetypeID : IEquatable<AnimalArchetypeID>
{
    public readonly ushort Value;

    public AnimalArchetypeID(ushort value)
    {
        Value = value;
    }

    public bool Equals(AnimalArchetypeID other) => Value == other.Value;

    public override bool Equals(object obj) => obj is AnimalArchetypeID other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(AnimalArchetypeID left, AnimalArchetypeID right) => left.Equals(right);

    public static bool operator !=(AnimalArchetypeID left, AnimalArchetypeID right) => !left.Equals(right);
}