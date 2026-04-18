using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPreviewController : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.RawImage _rawImageHexPreview;

    public Dictionary<MapModeTypes, Texture2D> MapModeTextures = new Dictionary<MapModeTypes, Texture2D>();

    private void SwitchMapMode(MapModeTypes type)
    {
        _rawImageHexPreview.texture = MapModeTextures[type];
    }

    public void SetGeneralMapMode()
    {
        SwitchMapMode(MapModeTypes.General);
    }

    public void SetElevationMapMode()
    {
        SwitchMapMode(MapModeTypes.Elevation);
    }

    public void SetTemperatureMapMode()
    {
        SwitchMapMode(MapModeTypes.Temperature);
    }

    public void SetPrecipitationMapMode()
    {
        SwitchMapMode(MapModeTypes.Precipitation);
    }

    public void SetLowVegetationMapMode()
    {
        SwitchMapMode(MapModeTypes.LowVegetation);
    }

    public void SetHighVegetationMapMode()
    {
        SwitchMapMode(MapModeTypes.HighVegetation);
    }
}
