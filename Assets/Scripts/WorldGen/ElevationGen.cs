using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ElevationGen
{
    public static void GenerateHeightmap(HexGrid grid, int seed, FBMLayerInformation baseLayer, List<FBMLayerInformation> detailLayers, float widthHeightRatio)
    {
            
        baseLayer.OriginPoint = SeedToVector2(seed);
        baseLayer.BoundPoint = baseLayer.OriginPoint + new Vector2(baseLayer.LayerParams.FractalWidthSpan, baseLayer.LayerParams.FractalWidthSpan / widthHeightRatio);
        baseLayer.CoordMap = AxialGeometry.ConvertAxialSetToBoundedCartesian(grid.GetAllAxialCoords(), baseLayer.OriginPoint, baseLayer.BoundPoint, out float size);
        baseLayer.ElevationMap = new();

        foreach (var layer in detailLayers)
        {
            layer.OriginPoint = SeedToVector2(seed);
            layer.BoundPoint = layer.OriginPoint + new Vector2(layer.LayerParams.FractalWidthSpan, layer.LayerParams.FractalWidthSpan / widthHeightRatio);
            layer.CoordMap = AxialGeometry.ConvertAxialSetToBoundedCartesian(grid.GetAllAxialCoords(), layer.OriginPoint, layer.BoundPoint, out float hexSize);
            layer.ElevationMap = new();
            
            float minElevation = float.MaxValue;
            float maxElevation = float.MinValue;
            
            foreach (HexData data in grid.GetValidHexes())
            {
                layer.ElevationMap[data.Coord] = FractalBrownianMotion.FBM(layer.CoordMap[data.Coord], layer.LayerParams);
                if (layer.ElevationMap[data.Coord] > maxElevation) maxElevation = layer.ElevationMap[data.Coord];
                if (layer.ElevationMap[data.Coord] < minElevation) minElevation = layer.ElevationMap[data.Coord];
            }
            
            NormalizeElevationMap(layer.ElevationMap, minElevation, maxElevation);
        }

        foreach (HexData data in grid.GetValidHexes())
        {
            float elevation = FractalBrownianMotion.FBM(baseLayer.CoordMap[data.Coord], baseLayer.LayerParams);
            elevation = elevation > 0.5f ? baseLayer.LayerWeight : 0f;
            baseLayer.ElevationMap[data.Coord] = elevation;
        }

        foreach (HexData data in grid.GetValidHexes())
        {
            float layerSum = baseLayer.ElevationMap[data.Coord];
            foreach (var layer in detailLayers)
            {
                if (layerSum > 0f)
                {
                    layerSum += layer.ElevationMap[data.Coord] * layer.LayerWeight;
                }
            }
            //flatten slightly
            layerSum = Mathf.Pow(layerSum, 1.5f);
            data.ExtraData.SetElevation(layerSum);
        }
    }

    private static void DebugMinMax(Dictionary<AxialCoordinate, float> map)
    {
        float min = float.MaxValue;
        float max = float.MinValue;
        
        var keys = map.Keys;

        foreach (var key in keys)
        {
            if (map[key] >  max) max = map[key];
            if (map[key] <  min) min = map[key];
        }
        
        Debug.Log("Max: " + max + " Min: " + min);
    }
    
    public static void NormalizeMap(HexGrid grid, float highestElevation)
    {
        foreach (HexData data in grid.GetValidHexes())
        {
            data.ExtraData.SetElevation(data.ExtraData.Elevation / highestElevation);
        }
    }

    public static void NormalizeElevationMap(Dictionary<AxialCoordinate, float> map, float maxElevation,
        float minElevation)
    {
        var keys = new List<AxialCoordinate>(map.Keys);
        foreach (var key in keys)
        {
            map[key] = (map[key] - minElevation) / (maxElevation - minElevation);
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