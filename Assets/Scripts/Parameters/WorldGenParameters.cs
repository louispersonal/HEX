using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenParameters", menuName = "ScriptableObjects/WorldGen", order = 1)]

public class WorldGenParameters : ScriptableObject
{
    public float CoastalDenominator;
    public float MinimumElevationRiverSource;
    public float MinimumPrecipitationRiverSource;
    public float MaximumRiverLengthRatio;
    public float RiverSpacing;
    public float MaxPrecipitationDistanceFromSea;
    public float MaxRegionSizeRatio;
}
