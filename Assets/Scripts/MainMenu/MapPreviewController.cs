using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapPreviewController : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.RawImage _rawImageHexPreview;

    [SerializeField] private TextMeshProUGUI _mapPreviewName;

    public Dictionary<MapModeTypes, Texture2D> MapModeTextures = new Dictionary<MapModeTypes, Texture2D>();

    private void SwitchMapMode(MapModeTypes type)
    {
        _rawImageHexPreview.texture = MapModeTextures[type];
    }

    public void SetMapName(string name)
    {
        _mapPreviewName.text = name;
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

    public void SetGeoFeatureMapMode()
    {
        SwitchMapMode(MapModeTypes.GeoFeatures);
    }

    public void SetRegionMapMode()
    {
        SwitchMapMode(MapModeTypes.Regions);
    }
}
