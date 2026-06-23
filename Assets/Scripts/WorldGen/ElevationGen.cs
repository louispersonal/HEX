using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ElevationGen
{
    public static void GenerateHeightmap(HexGrid grid, int seed, List<FBMLayerInformation> layers, float widthHeightRatio)
    {
        float highestElevation = 0f;
        
        foreach (var layer in layers)
        {
            var fbmParams = layer.LayerParams;
            
            Vector2 _originPoint = SeedToVector2(seed);
            Vector2 _boundPoint = _originPoint + new Vector2(fbmParams.FractalWidthSpan, fbmParams.FractalWidthSpan / widthHeightRatio);

            var coords = AxialGeometry.ConvertAxialSetToBoundedCartesian(grid.GetAllAxialCoords(), _originPoint, _boundPoint, out float size);
            
            foreach (HexData data in grid.GetValidHexes())
            {
                float elevation = layer.LayerWeight * FractalBrownianMotion.FBM(coords[data.Coord], fbmParams);
                elevation = elevation > 0.5 ? (elevation - 0.5f) * 2f : 0f;
                if (elevation > highestElevation) highestElevation = elevation;
                data.ExtraData.SetElevation(elevation);
            }
        }
        
        NormalizeHeightmap(grid, highestElevation);
    }

    public static void NormalizeHeightmap(HexGrid grid, float highestElevation)
    {
        foreach (HexData data in grid.GetValidHexes())
        {
            float newElevation = data.ExtraData.Elevation / highestElevation;
            data.ExtraData.SetElevation(newElevation);
        }
    }

    public static Vector2 SeedToVector2(int seed)
    {
        uint hash = (uint)seed;

        // Mix once for X
        hash ^= 0x9E3779B9u;
        hash *= 0x85EBCA6Bu;
        hash ^= hash >> 16;

        float x = hash / (float)uint.MaxValue;

        // Mix again for Y
        hash ^= 0xC2B2AE35u;
        hash *= 0x27D4EB2Fu;
        hash ^= hash >> 15;

        float y = hash / (float)uint.MaxValue;

        return new Vector2(x * 10000f, y * 10000f);
    }
}

[System.Serializable]
public class FBMLayerInformation
{
    public FractalBrownianMotionParameters LayerParams;
    public float LayerWeight;
}