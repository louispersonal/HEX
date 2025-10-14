using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerationMenuController : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.RawImage _previewImage;
    [SerializeField] FractalBrownianMotion _fractalBrownianMotion;

    private FBMParams _fbmParams = new FBMParams();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSeed(string value)
    {
        _fbmParams._seed = int.Parse(value);
    }

    public void UpdateWorldWidth(string value)
    {
        _fbmParams._worldWidth = int.Parse(value);
    }

    public void UpdateWorldHeight(string value)
    {
        _fbmParams._worldHeight = int.Parse(value);
    }

    public void UpdateOctaves(float value)
    {
        _fbmParams._octaves = (int)value;
    }

    public void UpdateLacunarity(float value)
    {
        _fbmParams._lacunarity = value;
    }

    public void UpdateGain(float value)
    {
        _fbmParams._gain = value;
    }

    public void UpdateSeaLevel(float value)
    {
        _fbmParams._seaLevel = value;
    }

    public void UpdateArchipelagoness(float value)
    {
        _fbmParams._archipelagoness = value;
    }

    public void Generate()
    {
        Texture2D previewTexture = _fractalBrownianMotion.GenerateContinentsPreview(_fbmParams);
        _previewImage.texture = previewTexture;
    }
}
