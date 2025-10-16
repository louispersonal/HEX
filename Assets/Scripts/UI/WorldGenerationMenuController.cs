using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldGenerationMenuController : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.RawImage _previewImage;
    [SerializeField] FractalBrownianMotion _fractalBrownianMotion;

    private FBMParams _fbmParams = new FBMParams();

    // UI Elements
    [Header("UI Elements")]
    [SerializeField] TMP_InputField _seedField;
    [SerializeField] TMP_InputField _widthField;
    [SerializeField] Slider _octavesSlider;
    [SerializeField] Slider _lacunaritySlider;
    [SerializeField] Slider _gainSlider;
    [SerializeField] Slider _seaLevelSlider;
    [SerializeField] Slider _archipelagonessSlider;

    // Start is called before the first frame update
    void Start()
    {
        SetRecommendedParams();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRecommendedParams()
    {
        _fbmParams._octaves = 5;
        _octavesSlider.value = _fbmParams._octaves;

        _fbmParams._lacunarity = 1.5f;
        _lacunaritySlider.value = _fbmParams._lacunarity;

        _fbmParams._gain = 0.8f;
        _gainSlider.value = _fbmParams._gain;

        _fbmParams._seed = 123;
        _seedField.text = _fbmParams._seed.ToString();

        _fbmParams._seaLevel = 0.5f;
        _seaLevelSlider.value = _fbmParams._seaLevel;

        _fbmParams._worldWidth = 200;
        _widthField.text = _fbmParams._worldWidth.ToString();

        _fbmParams._worldHeight = _fbmParams._worldWidth / 2;

        _fbmParams._archipelagoness = 3;
        _archipelagonessSlider.value = _fbmParams._archipelagoness;
    }

    public void UpdateSeed(string value)
    {
        _fbmParams._seed = int.Parse(value);
    }

    public void UpdateWorldWidth(string value)
    {
        _fbmParams._worldWidth = int.Parse(value);
        _fbmParams._worldHeight = _fbmParams._worldWidth / 2;
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

    public void ReturnToMainMenu()
    {
        GameController.Instance.GoToScene(SceneNames.MainMenu);
    }
}
