using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HexPanel : Panel
{
    private Hex _hex;

    [SerializeField] private Image _skyImage;
    [SerializeField] private Image _groundImage;
    
    [SerializeField] private Sprite[]  _skySprites;
    [SerializeField] private Sprite[] _groundSprites;
    
    [SerializeField] private TextMeshProUGUI _biomeText;
    [SerializeField] private TextMeshProUGUI _lowVegetationText;
    [SerializeField] private TextMeshProUGUI _highVegetationText;
    [SerializeField] private TextMeshProUGUI _temperatureText;
    [SerializeField] private TextMeshProUGUI _precipitationText;
    [SerializeField] private TextMeshProUGUI _elevationText;

    public void SetData(Hex hex)
    {
        _hex = hex;
    }
    
    public void UpdatePanel()
    {
        if (_hex == null)
        {
            return;
        }
        
        _skyImage.sprite = GetSkySprite();
        _groundImage.sprite = GetGroundSprite();
        
        _biomeText.text = _hex.ExtraData.Biome.ToString();
        _lowVegetationText.text = GetVegetationText(_hex.ExtraData.LowVegetation);
        _highVegetationText.text = GetVegetationText(_hex.ExtraData.HighVegetation);
        _temperatureText.text = _hex.ExtraData.GetTemperatureInDegrees().ToString("0.#") + "°C";
        _precipitationText.text = _hex.ExtraData.GetPrecipitationInMMs().ToString("0.#") + "mm";
        _elevationText.text = _hex.ExtraData.GetElevationInMeters().ToString("0") + "m";
    }

    private string GetVegetationText(float vegetation)
    {
        if (vegetation < 0.33f)
        {
            return "Scarce";
        }
        if (vegetation < 0.66f)
        {
            return "Moderate";
        }

        return "Abundant";
    }

    private Sprite GetGroundSprite()
    {
        return _groundSprites[(int)_hex.ExtraData.Biome];
    }

    private Sprite GetSkySprite()
    {
        if (_hex.ExtraData.Precipitation < 0.33f)
        {
            return _skySprites[0];
        }
        if (_hex.ExtraData.Precipitation < 0.66f)
        {
            return _skySprites[1];
        }

        return _skySprites[2];
    }
}
