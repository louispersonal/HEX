using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class FractalBrownianMotion : MonoBehaviour
{
    [Range(3, 10)] [SerializeField] int _octaves;
    [Range(1f, 2f)] [SerializeField] float _lacunarity;
    [Range(0f, 0.999f)] [SerializeField] float _gain;
    [SerializeField] float _seed;
    [Range(0, 1)] [SerializeField] float _seaLevel;
    [SerializeField] int _worldWidth;
    [SerializeField] int _worldHeight;
    [Range(2f, 8f)] [SerializeField] float _archipelagoness;

    [SerializeField] FBMParams _fbmParams;

    private const float SEED_Y_OFFSET = 1.37f;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Generate()
    {
        RecalculateNoise();
    }

    void RecalculateNoise()
    {
        float baseAmplitude = (1 - _gain) / (1 - Mathf.Pow(_gain, _octaves));
        float baseFrequency = _archipelagoness / _worldWidth;

        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(_worldWidth, _worldHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;

        // For each pixel in the texture...
        for (float y = 0.0F; y < noiseTex.height; y++)
        {
            for (float x = 0.0F; x < noiseTex.width; x++)
            {
                float sample = FBM(new Vector2(x, y), baseAmplitude, baseFrequency, _octaves, _lacunarity, _gain, _seed, true);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    public float FBM(Vector2 samplePoint, float baseAmplitude, float baseFrequency, int octaves, float lacunarity, float gain, float seed, bool binarySnap)
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
            if (fbm < _seaLevel)
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