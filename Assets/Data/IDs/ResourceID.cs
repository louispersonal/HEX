using System;

[Serializable]
public readonly struct ResourceID : IEquatable<ResourceID>
{
    public readonly ushort Value;

    public ResourceID(ushort value)
    {
        Value = value;
    }

    public bool Equals(ResourceID other) => Value == other.Value;

    public override bool Equals(object obj) => obj is ResourceID other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ResourceID left, ResourceID right) => left.Equals(right);

    public static bool operator !=(ResourceID left, ResourceID right) => !left.Equals(right);
}