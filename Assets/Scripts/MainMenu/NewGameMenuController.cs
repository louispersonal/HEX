using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameMenuController : SubMenu
{
    NewGameMenuView NewGameView { get { return _subMenuView as NewGameMenuView; } }

    public void GenerateWorld(int width, int seed)
    {
        StartCoroutine(WaitForWorldGenerationCoroutine(width, seed));
    }

    private IEnumerator WaitForWorldGenerationCoroutine(int width, int seed)
    {
        WorldGenController _worldGenController = GameController.Instance.WorldGenController;
        _worldGenController.StartCoroutine(_worldGenController.GenerateWorldData(width, seed));

        while (_worldGenController.GenerationInProgress)
        {
            NewGameView.WorldGenLoadingPanel.UpdateStatus(_worldGenController.AmountDone, _worldGenController.CurrentStatus);
            yield return null;
        }

        SetWorldAsCurrent(_worldGenController.NewWorld);

        Color[] generalPixels = TextureUtilities.GetPixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);
        Texture2D generalTexture = TextureUtilities.GetTexture(generalPixels, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        Color[] elevationPixels = TextureUtilities.GetMapModePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1],
            Color.white, MapModeTypes.Elevation);
        Texture2D elevationTexture = TextureUtilities.GetTexture(elevationPixels, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        Color[] temperaturePixels = TextureUtilities.GetMapModePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1],
            Color.red, MapModeTypes.Temperature);
        Texture2D temperatureTexture = TextureUtilities.GetTexture(temperaturePixels, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        Color[] precipitationPixels = TextureUtilities.GetMapModePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1],
            Color.blue, MapModeTypes.Precipitation);
        Texture2D precipitationTexture = TextureUtilities.GetTexture(precipitationPixels, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        Color[] lowVegetationPixels = TextureUtilities.GetMapModePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1],
            Color.green, MapModeTypes.LowVegetation);
        Texture2D lowVegetationTexture = TextureUtilities.GetTexture(lowVegetationPixels, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        Color[] highVegetationPixels = TextureUtilities.GetMapModePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1],
            Color.green, MapModeTypes.HighVegetation);
        Texture2D highVegetationTexture = TextureUtilities.GetTexture(highVegetationPixels, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        Color[] geoFeaturePixels = TextureUtilities.GetGeoFeaturePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);
        Texture2D geoFeatureTexture = TextureUtilities.GetTexture(geoFeaturePixels, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        Color[] regionPixels = TextureUtilities.GetRegionPixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);
        Texture2D regionTexture = TextureUtilities.GetTexture(regionPixels, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        NewGameView.MapPreview.MapModeTextures[MapModeTypes.General] = generalTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.Elevation] = elevationTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.Temperature] = temperatureTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.Precipitation] = precipitationTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.LowVegetation] = lowVegetationTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.HighVegetation] = highVegetationTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.GeoFeatures] = geoFeatureTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.Regions] = regionTexture;

        NewGameView.MapPreview.SetGeneralMapMode();
    }

    private void SetWorldAsCurrent(WorldData worldData)
    {
        GameController.Instance.SessionManager.SetWorldData(worldData);
    }
}
