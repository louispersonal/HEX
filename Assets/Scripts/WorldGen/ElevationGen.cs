using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ElevationGen
{
    public static void GenerateHeightmap(HexGrid grid, int seed, List<FBMLayerInformation> layers, float widthHeightRatio)
    {
        foreach (var layer in layers)
        {
            layer.OriginPoint = SeedToVector2(seed);
            layer.BoundPoint = layer.OriginPoint + new Vector2(layer.LayerParams.FractalWidthSpan, layer.LayerParams.FractalWidthSpan / widthHeightRatio);
            layer.CoordMap = AxialGeometry.ConvertAxialSetToBoundedCartesian(grid.GetAllAxialCoords(), layer.OriginPoint, layer.BoundPoint, out float size);
            layer.ElevationMap = new();
            
            foreach (HexData data in grid.GetValidHexes())
            {
                layer.ElevationMap[data.Coord] = FractalBrownianMotion.FBM(layer.CoordMap[data.Coord], layer.LayerParams);
            }
        }

        float highestElevation = 0f;
        
        foreach (HexData data in grid.GetValidHexes())
        {
            float layerSum = 0f;
            foreach (var layer in layers)
            {
                layerSum += layer.ElevationMap[data.Coord] * layer.LayerWeight;
            }
            layerSum = layerSum > 0.5f ? (layerSum - 0.5f) * 2f : 0f;
            if (layerSum > highestElevation) highestElevation = layerSum;
            data.ExtraData.SetElevation(layerSum);
        }
        
        NormalizeMap(grid,  highestElevation);
    }

    public static void NormalizeMap(HexGrid grid, float highestElevation)
    {
        foreach (HexData data in grid.GetValidHexes())
        {
            data.ExtraData.SetElevation(data.ExtraData.Elevation / highestElevation);
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

    [HideInInspector] public Vector2 OriginPoint;
    [HideInInspector] public Vector2 BoundPoint;
    [HideInInspector] public Dictionary<AxialCoordinate, Vector2> CoordMap;
    [HideInInspector] public Dictionary<AxialCoordinate, float> ElevationMap;
}