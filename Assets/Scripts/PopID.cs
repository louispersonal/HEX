using System;

public class PopID : IEquatable<PopID>
{
    public ushort Value;

    public PopID(ushort value)
    {
        Value = value;
    }
    
    public bool Equals(PopID other) => Value == other.Value;

    public override bool Equals(object obj) => obj is PopID other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(PopID left, PopID right) => left.Equals(right);

    public static bool operator !=(PopID left, PopID right) => !left.Equals(right);
}
