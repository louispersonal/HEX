using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hex : BaseHex
{
    private const float ByteToFloat = 1f / 255f;

    [SerializeField] byte _lowVegetation;
    public float LowVegetation { get { return _lowVegetation * ByteToFloat; } }

    [SerializeField] byte _highVegetation;
    public float HighVegetation { get { return _highVegetation * ByteToFloat; } }

    [SerializeField] byte _temperature;
    public float Temperature { get { return _temperature * ByteToFloat; } }

    [SerializeField] byte _elevation;
    public float Elevation { get { return _elevation * ByteToFloat; } }

    [SerializeField] byte _precipitation;
    public float Precipitation { get { return _precipitation * ByteToFloat; } }

    public Hex(AxialCoordinate a) : base(a)
    {

    }

    public Hex(int q, int r) : base(q, r)
    {
        
    }

    public void PopulateData()
    {

    }

    public override string ToString()
    {
        return $"LV: {LowVegetation}, HV: {HighVegetation}, T: {Temperature}, E: {Elevation}, P: {Precipitation}";
    }

    public List<Hex> GetNeighbors()
    {
        List<Hex> neighbors = new List<Hex>();

        foreach (AxialCoordinate dir in AxialDirections.Directions)
        {
            AxialCoordinate neighborCoord = Coord + dir;
            if (BaseHexGrid.Instance.TryGetHex(neighborCoord, out Hex neighborHex))
            {
                neighbors.Add(neighborHex);
            }
        }

        return neighbors;
    }

    public float DistanceToHex(Hex target)
    {
        return BaseHexGrid.Instance.DistanceBetweenHexes(this, target);
    }

    public List<Hex> HexesWithinRadius(int radius)
    {
        return BaseHexGrid.Instance.HexesWithinRadiusOfHex(this, radius);
    }

    public Vector2 GetScenePosition()
    {
        return BaseHexGrid.Instance.AxialToSceneConversion(Coord);
    }
}
