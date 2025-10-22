using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class FractalBrownianMotion
{
    private const float SEED_Y_OFFSET = 1.37f;

    public static BaseHexGrid ApplyContinentHeightmap(FBMParams fbmParams, BaseHexGrid hexGrid)
    {
        float sampleSpacing = 0.6f; // this dictates the estimated hex size to allow sampling of the perlin noise to be spaced appropriately
        float baseAmplitude = (1 - fbmParams.Gain) / (1 - Mathf.Pow(fbmParams.Gain, fbmParams.Octaves));
        float baseFrequency = fbmParams.Archipelagoness / fbmParams.WorldWidth;

        foreach (Hex hex in hexGrid.Grid.Values)
        {
            Vector2 coord = AxialConverter.AxialToCartesianConversion(hex.Coord, sampleSpacing);
            float value = FBM(coord, baseAmplitude, baseFrequency, fbmParams.Octaves, fbmParams.Lacunarity, fbmParams.Gain, fbmParams.Seed, true, fbmParams.SeaLevel);
            hex.SetElevation(value);
        }

        return hexGrid;
    }
    public static Texture2D GenerateContinentsPreview(FBMParams fbmParams)
    {
        float baseAmplitude = (1 - fbmParams.Gain) / (1 - Mathf.Pow(fbmParams.Gain, fbmParams.Octaves));
        float baseFrequency = fbmParams.Archipelagoness / fbmParams.WorldWidth;

        int width = fbmParams.WorldWidth;
        int height = fbmParams.WorldHeight;

        Texture2D noiseTex = new Texture2D(width, height);
        Color[] pix = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // FBM sample stays the same
                float sample = FBM(
                    new Vector2(x, y),
                    baseAmplitude,
                    baseFrequency,
                    fbmParams.Octaves,
                    fbmParams.Lacunarity,
                    fbmParams.Gain,
                    fbmParams.Seed,
                    true,
                    fbmParams.SeaLevel
                );

                pix[y * width + x] = new Color(sample, sample, sample);
            }
        }

        noiseTex.SetPixels(pix);
        noiseTex.Apply();

        return noiseTex;
    }

    public static float FBM(Vector2 samplePoint, float baseAmplitude, float baseFrequency, int octaves, float lacunarity, float gain, float seed, bool binarySnap, float seaLevel)
    {
        float fbm = 0f;
        samplePoint += new Vector2(seed, seed * SEED_Y_OFFSET);

        for (int i = 0; i < octaves; i++)
        {
            fbm += Mathf.PerlinNoise(samplePoint.x * baseFrequency, samplePoint.y * baseFrequency) * baseAmplitude;
            baseFrequency *= lacunarity;
            baseAmplitude *= gain;
        }

        // if binarySnap is on, we set everything below sea to 0 and everything else to 0.01
        if (binarySnap)
        {
            if (fbm < seaLevel)
            {
                return 0f;
            }
            else
            {
                return 1f;
            }
        }

        return fbm;
    }
}