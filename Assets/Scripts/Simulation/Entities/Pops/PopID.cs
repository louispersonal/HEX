using System;
using UnityEngine;

[Serializable]
public struct PopID : IEquatable<PopID>
{
    [SerializeField] private ushort _value;

    public ushort Value => _value;

    public PopID(ushort value)
    {
        _value = value;
    }
    
    public bool Equals(PopID other) => Value == other.Value;

    public override bool Equals(object obj) => obj is PopID other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(PopID left, PopID right) => left.Equals(right);

    public static bool operator !=(PopID left, PopID right) => !left.Equals(right);
}
