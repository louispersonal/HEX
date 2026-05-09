using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GeoFeatureGen
{
    public static void AddGeoFeatures(WorldData world, List<GeoFeatureSpawnParameters> parameters)
    {
        Dictionary<GeoFeatureType, List<HexData>> candidateHexes = new Dictionary<GeoFeatureType, List<HexData>>();

        foreach (GeoFeatureSpawnParameters sp in parameters)
        {
            candidateHexes[sp.FeatureType] = new List<HexData>();
        }

        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (data.ExtraData.IsSea) continue;

            foreach (GeoFeatureSpawnParameters sp in parameters)
            {
                if (CheckSpawnable(world, data, sp))
                {
                    candidateHexes[sp.FeatureType].Add(data);

                }
            }
        }

        int geoFeatureIndex = 0;

        foreach (GeoFeatureSpawnParameters sp in parameters)
        {
            int numCandidates = candidateHexes[sp.FeatureType].Count;
            float chance = sp.TargetNumber / numCandidates;
            
            foreach (HexData data in candidateHexes[sp.FeatureType])
            {
                if (Random.Range(0f, 1f) < chance)
                {
                    GeoID geoID = new GeoID(geoFeatureIndex);
                    GeoFeature newGeoFeature = new GeoFeature(geoID, new List<AxialCoordinate> { data.Coord }, sp.FeatureType);
                    world.GeoFeatures.Add(geoID, newGeoFeature, new List<AxialCoordinate> { data.Coord });
                    geoFeatureIndex++;
                }
            }
        }
    }

    private static bool CheckSpawnable(WorldData world, HexData hex, GeoFeatureSpawnParameters parameter)
    {
        if (parameter.SeaAdjacentRequirement != Requirement.Any)
        {
            bool isSeaAdjacent = world.Grid.NumHexesFromSea(hex, out var hexData) == 1;
            if (!PassesRequirement(isSeaAdjacent, parameter.SeaAdjacentRequirement)) return false;
        }

        if (parameter.RiverRequirement != Requirement.Any)
        {
            bool hasRiver = world.Rivers.ContainsAt(hex.Coord);
            if (!PassesRequirement(hasRiver, parameter.RiverRequirement)) return false;
        }

        if (!parameter.AcceptedBiomes.Contains(hex.ExtraData.Biome)) return false;

        if (!(hex.ExtraData.Elevation >= parameter.ElevationMin && hex.ExtraData.Elevation <= parameter.ElevationMax)) return false;
        if (!(hex.ExtraData.Temperature >= parameter.TemperatureMin && hex.ExtraData.Temperature <= parameter.TemperatureMax)) return false;
        if (!(hex.ExtraData.Precipitation >= parameter.PrecipitationMin && hex.ExtraData.Precipitation <= parameter.PrecipitationMax)) return false;
        if (!(hex.ExtraData.LowVegetation >= parameter.LowVegetationMin && hex.ExtraData.LowVegetation <= parameter.LowVegetationMax)) return false;
        if (!(hex.ExtraData.HighVegetation >= parameter.HighVegetationMin && hex.ExtraData.HighVegetation <= parameter.HighVegetationMax)) return false;

        //float chance = parameter.TargetNumber / validHexes;
        //if (Random.Range(0f, 1f) >= chance) return false;

        return true;
    }

    private static bool PassesRequirement(bool value, Requirement requirement)
    {
        return requirement switch
        {
            Requirement.Any => true,
            Requirement.MustBeTrue => value,
            Requirement.MustBeFalse => !value,
            _ => true
        };
    }
}
  