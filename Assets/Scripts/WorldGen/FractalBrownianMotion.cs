using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class FractalBrownianMotion
{
    private const float SEED_Y_OFFSET = 1.37f;

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