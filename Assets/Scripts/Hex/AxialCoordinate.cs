using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AxialCoordinate : System.IEquatable<AxialCoordinate>
{
    [SerializeField] private int q;
    [SerializeField] private int r;

    public int Q { get { return q; } }
    public int R { get { return r; } }
    public int S { get { return -q - r; } }

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

    public static AxialCoordinate operator *(AxialCoordinate a, int i)
    {
        return new AxialCoordinate(a.Q * i, a.R * i);
    }
	
	public static float DistanceBetweenCoords(AxialCoordinate a, AxialCoordinate b)
    {
        AxialCoordinate diff = a - b;
        return (Mathf.Abs(diff.Q) + Mathf.Abs(diff.R) + Mathf.Abs(diff.Q + diff.R)) / 2;
    }
	
	public static List<AxialCoordinate> CoordsWithinRadiusOfCoord(AxialCoordinate a, int radius)
    {
		int count = 1 + 3 * radius * (radius + 1);
		
        List<AxialCoordinate> coordsInRange = new List<AxialCoordinate>(count);

        for (int q = -radius; q <= radius; q++)
        {
            for (int r = Mathf.Max(-radius, -q - radius); r <= Mathf.Min(radius, -q + radius); r++)
            {
                AxialCoordinate hexCoord = a + new AxialCoordinate(q, r);
				coordsInRange.Add(hexCoord);
            }
        }

        return coordsInRange;
    }
	
	public static List<AxialCoordinate> CoordsInRingOfRadius(AxialCoordinate a, int radius)
	{
        List<AxialCoordinate> outList = new List<AxialCoordinate>();
		if (radius == 0) { outList.Add(a); return outList; }

		// axial directions (pointy-top)
		AxialCoordinate[] dirs = AxialDirections.Directions;

		// start at (center + dir5 * radius)
		AxialCoordinate coord = a + new AxialCoordinate(0,1) * radius;
		for (int i = 0; i < 6; i++)
		{
			for (int step = 0; step < radius; step++)
			{
				outList.Add(coord);
				coord += dirs[i];
			}
		}
		
		return outList;
	}
	
	public static List<AxialCoordinate> CoordsInRingsOfRadii(AxialCoordinate a, int minRadius, int maxRadius)
	{
        List<AxialCoordinate> outList = new List<AxialCoordinate>();
		
		for (int r = minRadius; r <= maxRadius; r++)
		{
			outList.AddRange(CoordsInRingOfRadius(a, r));
		}
		
		return outList;
	}
}