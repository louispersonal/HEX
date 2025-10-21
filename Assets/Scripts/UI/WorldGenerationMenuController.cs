using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldGenerationMenuController : MonoBehaviour
{
    [SerializeField] MenuView _worldGenerationMenuView;

    [SerializeField] UnityEngine.UI.RawImage _previewImage;
    [SerializeField] FractalBrownianMotion _fractalBrownianMotion;

    public FBMParams FBMParams { get { return WorldManager.Instance.MapDefinition.FBMParams; } }

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

    public void EnableMenu()
    {
        _worldGenerationMenuView.gameObject.SetActive(true);
    }

    public void DisableMenu()
    {
        _worldGenerationMenuView.gameObject.SetActive(false);
    }

    public void SetRecommendedParams()
    {
        FBMParams.Octaves = 5;
        _octavesSlider.value = FBMParams.Octaves;

        FBMParams.Lacunarity = 1.5f;
        _lacunaritySlider.value = FBMParams.Lacunarity;

        FBMParams.Gain = 0.8f;
        _gainSlider.value = FBMParams.Gain;

        FBMParams.Seed = 123;
        _seedField.text = FBMParams.Seed.ToString();

        FBMParams.SeaLevel = 0.5f;
        _seaLevelSlider.value = FBMParams.SeaLevel;

        FBMParams.WorldWidth = 200;
        _widthField.text = FBMParams.WorldWidth.ToString();

        FBMParams.WorldHeight = FBMParams.WorldWidth / 2;

        FBMParams.Archipelagoness = 3;
        _archipelagonessSlider.value = FBMParams.Archipelagoness;
    }

    public void UpdateSeed(string value)
    {
        FBMParams.Seed = int.Parse(value);
    }

    public void UpdateWorldWidth(string value)
    {
        FBMParams.WorldWidth = int.Parse(value);
        FBMParams.WorldHeight = FBMParams.WorldWidth / 2;
    }

    public void UpdateOctaves(float value)
    {
        FBMParams.Octaves = (int)value;
    }

    public void UpdateLacunarity(float value)
    {
        FBMParams.Lacunarity = value;
    }

    public void UpdateGain(float value)
    {
        FBMParams.Gain = value;
    }

    public void UpdateSeaLevel(float value)
    {
        FBMParams.SeaLevel = value;
    }

    public void UpdateArchipelagoness(float value)
    {
        FBMParams.Archipelagoness = value;
    }

    public void Generate()
    {
        Texture2D previewTexture = FractalBrownianMotion.GenerateContinentsPreview(FBMParams);
        _previewImage.texture = previewTexture;
    }

    public void SetWorldName(string name)
    {
        WorldManager.Instance.MapDefinition.Name = name;
    }

    public void ReturnToMainMenu()
    {
        DisableMenu();
        MainMenuController.Instance.EnableMenu();
    }

    public void GenerateHeightMapGoToSeedMenu()
    {
        WorldManager.Instance.NewHexGrid();
        MainMenuController.Instance.GoToSeedMenu();
    }
}
