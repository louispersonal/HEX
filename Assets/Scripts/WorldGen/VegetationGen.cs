using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationGen : MonoBehaviour
{
    private const float OPTIMUM_TEMP = 0.66f;
    private const float OPTIMUM_PREC = 0.66f;

    public static void GenerateVegetation(HexGrid grid, WorldGenParameters parameters)
    {
        foreach (HexData data in grid.GetValidHexes())
        {
            if (data.ExtraData.IsSea) continue;
            float totalBiomass = (0.5f * InverseQuadraticCurve(data.ExtraData.Temperature, OPTIMUM_TEMP, 4f)) + (0.5f * InverseQuadraticCurve(data.ExtraData.Precipitation, OPTIMUM_PREC, 4f));
            data.ExtraData.SetVegetations(totalBiomass, totalBiomass);
        }
    }
    
    public static float InverseQuadraticCurve(float x, float apex, float tightness)
    {
        return 1f - (tightness * Mathf.Pow(x - apex, 2));
    }
}
