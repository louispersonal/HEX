using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationGen : MonoBehaviour
{
    private const float OPTIMUM_TEMP = 0.66f;
    private const float OPTIMUM_PREC = 0.66f;
    private const float RIVER_BONUS = 0.66f;

    public static void GenerateVegetation(WorldData world, WorldGenParameters parameters)
    {
        foreach (HexData data in world.Grid.GetValidHexes())
        {
            if (data.ExtraData.IsSea) continue;
            float availableWater = world.Rivers.ContainsAt(data.Coord) ? Mathf.Max(RIVER_BONUS, data.ExtraData.Precipitation) : data.ExtraData.Precipitation;
            float totalBiomass = (0.5f * InverseQuadraticCurve(data.ExtraData.Temperature, OPTIMUM_TEMP, 4f)) + (0.5f * InverseQuadraticCurve(availableWater, OPTIMUM_PREC, 4f));
            float proportionLowVegetation = 0.5f * data.ExtraData.Elevation + 0.5f * world.Grid.GetWindDirection(data.Coord).magnitude;
            float proportionHighVegetation = 1f - proportionLowVegetation;
            data.ExtraData.SetVegetations(totalBiomass * proportionLowVegetation, totalBiomass * proportionHighVegetation);
        }
    }
    
    public static float InverseQuadraticCurve(float x, float apex, float tightness)
    {
        return 1f - (tightness * Mathf.Pow(x - apex, 2));
    }
}
