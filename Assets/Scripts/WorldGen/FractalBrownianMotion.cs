using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class FractalBrownianMotion
{
    public static float FBM(Vector2 samplePoint, FractalBrownianMotionParameters FBMParams)
    {
        float fbm = 0f;

        float frequency = FBMParams.BaseFrequency;
        float amplitude = (1 - FBMParams.Gain) / (1 - Mathf.Pow(FBMParams.Gain, FBMParams.Octaves));

        for (int i = 0; i < FBMParams.Octaves; i++)
        {
            fbm += Mathf.PerlinNoise(samplePoint.x * frequency, samplePoint.y * frequency) * amplitude;
            frequency *= FBMParams.Lacunarity;
            amplitude *= FBMParams.Gain;
        }

        return fbm;
    }

    public static float[] FBMSampleArea(Vector2 originPoint, Vector2 boundPoint, int numHorizontalSamplePoints, int numVerticalSamplePoints, FractalBrownianMotionParameters FBMParams)
    {
        float[] samplePoints = new float[numHorizontalSamplePoints * numVerticalSamplePoints];

        float horizontalSampleInterval = (boundPoint.x - originPoint.x) / (numHorizontalSamplePoints - 1);
        float verticalSampleInterval = (boundPoint.y - originPoint.y) / (numVerticalSamplePoints - 1);

        // the origin point (bottom left) and bound point (top right) form a rectangle bounding the sample space
        for (int j = 0; j < numVerticalSamplePoints; j++)
        {
            for (int i = 0; i < numHorizontalSamplePoints; i++)
            {
                Vector2 currentSample = new Vector2(originPoint.x + i * horizontalSampleInterval, originPoint.y + j * verticalSampleInterval);
                float sampleValue = FBM(currentSample, FBMParams);
                samplePoints[numHorizontalSamplePoints * j + i] = sampleValue;
            }
        }

        return samplePoints;
    }
}

[System.Serializable]
public class FractalBrownianMotionParameters
{
    public float BaseFrequency;
    public int Octaves;
    public float Lacunarity;
    public float Gain;
    public float FractalWidthSpan;

    public FractalBrownianMotionParameters(float baseFrequency, int octaves, float lacunarity, float gain, float fractalWidthSpan)
    {
        BaseFrequency = baseFrequency;
        Octaves = octaves;
        Lacunarity = lacunarity;
        Gain = gain;
        FractalWidthSpan = fractalWidthSpan;
    }
}