using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class FractalBrownianMotion : MonoBehaviour
{
    [SerializeField] FBMParams _fbmParams;

    private const float SEED_Y_OFFSET = 1.37f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Texture2D GenerateContinentsPreview(FBMParams fbmParams)
    {
        float baseAmplitude = (1 - fbmParams._gain) / (1 - Mathf.Pow(fbmParams._gain, fbmParams._octaves));
        float baseFrequency = fbmParams._archipelagoness / fbmParams._worldWidth;

        // Set up the texture and a Color array to hold pixels during processing.
        Texture2D noiseTex = new Texture2D(fbmParams._worldWidth, fbmParams._worldHeight);
        Color[] pix = new Color[noiseTex.width * noiseTex.height];

        // For each pixel in the texture...
        for (float y = 0.0F; y < noiseTex.height; y++)
        {
            for (float x = 0.0F; x < noiseTex.width; x++)
            {
                float sample = FBM(new Vector2(x, y), baseAmplitude, baseFrequency, fbmParams._octaves, fbmParams._lacunarity, fbmParams._gain, fbmParams._seed, true, fbmParams._seaLevel);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();

        return noiseTex;
    }

    public float FBM(Vector2 samplePoint, float baseAmplitude, float baseFrequency, int octaves, float lacunarity, float gain, float seed, bool binarySnap, float seaLevel)
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

[System.Serializable]
public class FBMParams
{
    public int _octaves;
    public float _lacunarity;
    public float _gain;
    public float _seed;
    public float _seaLevel;
    public int _worldWidth;
    public int _worldHeight;
    public float _archipelagoness;
}