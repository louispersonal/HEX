using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenParameters", menuName = "ScriptableObjects/GeoFeature", order = 2)]

public class GeoFeatureSpawnParameters : ScriptableObject
{
    public GeoFeatureType FeatureType;

    public List<Biome> AcceptedBiomes;
    public bool MustBeSeaAdjacent;

    public float ElevationMin;
    public float ElevationMax;

    public float TemperatureMin;
    public float TemperatureMax;

    public float PrecipitationMin;
    public float PrecipitationMax;

    public float LowVegetationMin;
    public float LowVegetationMax;

    public float HighVegetationMin;
    public float HighVegetationMax;

    public bool MustContainRiver;

    public float BaseChance;
}
