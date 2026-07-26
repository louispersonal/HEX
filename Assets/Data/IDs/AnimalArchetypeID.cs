using System;

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