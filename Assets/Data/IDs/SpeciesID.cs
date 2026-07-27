using System;

[Serializable]
public struct SpeciesID : IEquatable<SpeciesID>
{
    public ushort Value;

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