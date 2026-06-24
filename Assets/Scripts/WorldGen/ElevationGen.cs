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
            
            float highestElevationLayer = 0f;
            
            foreach (HexData data in grid.GetValidHexes())
            {
                float elevation = FractalBrownianMotion.FBM(layer.CoordMap[data.Coord], layer.LayerParams);
                if (layer.BaseLayer) elevation = elevation > 0.5 ? (elevation - 0.5f) * 2f : 0f;
                if (elevation > highestElevationLayer) highestElevationLayer = elevation;
                layer.ElevationMap[data.Coord] = elevation;
            }
            NormalizeElevationMap(layer.ElevationMap, highestElevationLayer, layer.LayerWeight);
        }
        
        foreach (HexData data in grid.GetValidHexes())
        {
            float layerSum = 0f;
            foreach (var layer in layers)
            {
                if (layer.BaseLayer || layerSum > 0) layerSum += layer.ElevationMap[data.Coord];
            }
            data.ExtraData.SetElevation(layerSum);
        }
    }

    public static void NormalizeElevationMap(Dictionary<AxialCoordinate, float> elevationMap, float highestElevation, float weight=1)
    {
        if (highestElevation <= 0f)
            return;

        var keys = new List<AxialCoordinate>(elevationMap.Keys);

        foreach (var key in keys)
        {
            elevationMap[key] = (elevationMap[key] / highestElevation) * weight;
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
    public bool BaseLayer;

    [HideInInspector] public Vector2 OriginPoint;
    [HideInInspector] public Vector2 BoundPoint;
    [HideInInspector] public Dictionary<AxialCoordinate, Vector2> CoordMap;
    [HideInInspector] public Dictionary<AxialCoordinate, float> ElevationMap;
}