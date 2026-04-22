using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GeoFeatureGen : MonoBehaviour
{
    public static void AddGeoFeatures(WorldData world, List<GeoFeatureSpawnParameters> parameters)
    {
        foreach(GeoFeatureSpawnParameters sp in parameters)
        {
            AddGeoFeature(world, sp.FeatureType, sp);
        }
    }

    private static void AddGeoFeature(WorldData world, GeoFeatureType type, GeoFeatureSpawnParameters parameter)
    {
        int geoFeatureIndex = 0;
        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (data.ExtraData.IsSea) continue;

            if (CheckSpawnable(world, data, parameter))
            {
                GeoID geoID = new GeoID(geoFeatureIndex);
                GeoFeature newGeoFeature = new GeoFeature(geoID, new List<AxialCoordinate> { data.Coord }, type);
                world.GeoFeatures.Add(geoID, newGeoFeature, new List<AxialCoordinate> { data.Coord });
                geoFeatureIndex++;
            }
        }
    }

    private static bool CheckSpawnable(WorldData world, HexData hex, GeoFeatureSpawnParameters parameter)
    {
        bool geoFeaturePresent = world.GeoFeatures.ContainsAt(hex.Coord);
        bool isSeaAdjacent = world.Grid.NumHexesFromSea(hex, out var hexData) == 1;
        return !geoFeaturePresent && 
            parameter.AcceptedBiomes.Contains(hex.ExtraData.Biome) &&
            isSeaAdjacent == parameter.MustBeSeaAdjacent &&
            hex.ExtraData.Elevation >= parameter.ElevationMin && hex.ExtraData.Elevation <= parameter.ElevationMax &&
            hex.ExtraData.Temperature >= parameter.TemperatureMin && hex.ExtraData.Temperature <= parameter.TemperatureMax &&
            hex.ExtraData.Precipitation >= parameter.PrecipitationMin && hex.ExtraData.Precipitation <= parameter.PrecipitationMax &&
            hex.ExtraData.LowVegetation >= parameter.LowVegetationMin && hex.ExtraData.LowVegetation <= parameter.LowVegetationMax &&
            hex.ExtraData.HighVegetation >= parameter.HighVegetationMin && hex.ExtraData.HighVegetation <= parameter.HighVegetationMax &&
            world.Rivers.ContainsAt(hex.Coord) == parameter.MustContainRiver &&
            Random.Range(0f, 1f) < parameter.BaseChance;
    }
}
  