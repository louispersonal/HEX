using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ElevationGen
{
    private const float WIDTH_HEIGHT_RATIO = 1.5f;
    private const int FRACTAL_WIDTH_SPAN = 200;

    public static void GenerateHeightmap(HexGrid grid, int seed, FractalBrownianMotionParameters fbmParams)
    {
        Vector2 _originPoint = SeedToVector2(seed);
        Vector2 _boundPoint = _originPoint + new Vector2(FRACTAL_WIDTH_SPAN, FRACTAL_WIDTH_SPAN / WIDTH_HEIGHT_RATIO);

        var coords = AxialGeometry.ConvertAxialSetToBoundedCartesian(grid.Grid.Keys.ToList(), _originPoint, _boundPoint, out float size);

        foreach (HexData data in grid.Grid.Values)
        {
            float elevation = FractalBrownianMotion.FBM(coords[data.Coord], fbmParams);
            elevation = elevation > 0.5 ? (elevation - 0.5f) * 2f : 0f;
            data.ExtraData.SetElevation(elevation);
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
