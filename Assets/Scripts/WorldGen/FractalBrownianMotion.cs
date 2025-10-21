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
        float baseAmplitude = (1 - fbmParams.Gain) / (1 - Mathf.Pow(fbmParams.Gain, fbmParams.Octaves));
        float baseFrequency = fbmParams.Archipelagoness / fbmParams.WorldWidth;

        foreach (Hex hex in hexGrid.Grid.Values)
        {
            (int x, int y) oCoord = hex.Coord.ConvertToOddR();
            float value = FBM(new Vector2(oCoord.x, oCoord.y), baseAmplitude, baseFrequency, fbmParams.Octaves, fbmParams.Lacunarity, fbmParams.Gain, fbmParams.Seed, true, fbmParams.SeaLevel);
            hex.SetElevation(value);
        }

        return hexGrid;
    }
    public static Texture2D GenerateContinentsPreview(FBMParams fbmParams)
    {
        float baseAmplitude = (1 - fbmParams.Gain) / (1 - Mathf.Pow(fbmParams.Gain, fbmParams.Octaves));
        float baseFrequency = fbmParams.Archipelagoness / fbmParams.WorldWidth;

        // Set up the texture and a Color array to hold pixels during processing.
        Texture2D noiseTex = new Texture2D(fbmParams.WorldWidth, fbmParams.WorldHeight);
        Color[] pix = new Color[noiseTex.width * noiseTex.height];

        // For each pixel in the texture...
        for (float y = 0.0F; y < noiseTex.height; y++)
        {
            for (float x = 0.0F; x < noiseTex.width; x++)
            {
                float sample = FBM(new Vector2(x, y), baseAmplitude, baseFrequency, fbmParams.Octaves, fbmParams.Lacunarity, fbmParams.Gain, fbmParams.Seed, true, fbmParams.SeaLevel);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        // Copy the pixel data to the texture and load it into the GPU.
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