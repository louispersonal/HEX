using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HexPanel : Panel
{
    private Hex _hex;

    [SerializeField] private TextMeshProUGUI _biomeText;
    [SerializeField] private TextMeshProUGUI _lowVegetationText;
    [SerializeField] private TextMeshProUGUI _highVegetationText;
    [SerializeField] private TextMeshProUGUI _temperatureText;
    [SerializeField] private TextMeshProUGUI _precipitationText;
    [SerializeField] private TextMeshProUGUI _elevationText;
    
    public void Initialize(Hex hex)
    {
        _hex = hex;
        Initialized = true;
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        _biomeText.text = _hex.ExtraData.Biome.ToString();
        _lowVegetationText.text = GetVegetationText(_hex.ExtraData.LowVegetation);
        _highVegetationText.text = GetVegetationText(_hex.ExtraData.HighVegetation);
        _temperatureText.text = _hex.ExtraData.GetTemperatureInDegrees().ToString("0.#") + "°C";
        _precipitationText.text = _hex.ExtraData.GetPrecipitationInMMs().ToString("0.#") + "mm";
        _elevationText.text = _hex.ExtraData.GetElevationInMeters().ToString("0") + "m";
    }

    public void Terminate()
    {
        Initialized = false;
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
}
