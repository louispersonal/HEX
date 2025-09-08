using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public readonly struct AxialCoordinate : System.IEquatable<AxialCoordinate>
{
    [SerializeField] private readonly int q;
    [SerializeField] private readonly int r;

    public int Q => q;
    public int R => r;
    public int S => -q - r;

    public AxialCoordinate(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    public bool Equals(AxialCoordinate other) => q == other.q && r == other.r;
    public override bool Equals(object obj) => obj is AxialCoordinate other && Equals(other);
    public override int GetHashCode() => System.HashCode.Combine(q, r);
    public override string ToString() => $"({Q}, {R}, {S})";

    public static AxialCoordinate operator + (AxialCoordinate a, AxialCoordinate b)
    {
        return new AxialCoordinate(a.Q + b.Q, a.R + b.R);
    }

    public static AxialCoordinate operator -(AxialCoordinate a, AxialCoordinate b)
    {
        return new AxialCoordinate(a.Q - b.Q, a.R - b.R);
    }
}