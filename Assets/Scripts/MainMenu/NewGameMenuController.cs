using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameMenuController : SubMenu
{
    NewGameMenuView NewGameView { get { return _subMenuView as NewGameMenuView; } }

    [HideInInspector] public List<string> WorldSaveFilenames;

    public void GenerateWorld(int width, int seed, string name)
    {
        StartCoroutine(WaitForWorldGenerationCoroutine(width, seed, name));
    }

    private IEnumerator WaitForWorldGenerationCoroutine(int width, int seed, string name)
    {
        WorldGenController _worldGenController = GameController.Instance.WorldGenController;
        _worldGenController.StartCoroutine(_worldGenController.GenerateWorldData(width, seed, name));

        while (_worldGenController.GenerationInProgress)
        {
            NewGameView.WorldGenLoadingPanel.UpdateStatus(_worldGenController.AmountDone, _worldGenController.CurrentStatus);
            yield return null;
        }

        SetWorldAsCurrent(_worldGenController.NewWorld);

        PreviewCurrentWorld();
        
        GameController.Instance.SessionManager.UiData = new UiData(NewGameView.MapPreview.MapModeTextures[MapModeTypes.General]);
    }

    private void PreviewCurrentWorld()
    {
        MapTexture generalTexture  = TextureUtilities.GetMapTextureFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        MapTexture elevationTexture = TextureUtilities.GetMapModePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1],
            Color.white, MapModeTypes.Elevation);

        MapTexture temperatureTexture = TextureUtilities.GetMapModePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1],
            Color.red, MapModeTypes.Temperature);

        MapTexture precipitationTexture = TextureUtilities.GetMapModePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1],
            Color.blue, MapModeTypes.Precipitation);

        MapTexture lowVegetationTexture = TextureUtilities.GetMapModePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1],
            Color.green, MapModeTypes.LowVegetation);

        MapTexture highVegetationTexture = TextureUtilities.GetMapModePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1],
            Color.green, MapModeTypes.HighVegetation);

        MapTexture geoFeatureTexture = TextureUtilities.GetGeoFeaturePixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        MapTexture regionTexture = TextureUtilities.GetRegionPixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);

        NewGameView.MapPreview.MapModeTextures[MapModeTypes.General] = generalTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.Elevation] = elevationTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.Temperature] = temperatureTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.Precipitation] = precipitationTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.LowVegetation] = lowVegetationTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.HighVegetation] = highVegetationTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.GeoFeatures] = geoFeatureTexture;
        NewGameView.MapPreview.MapModeTextures[MapModeTypes.Regions] = regionTexture;

        NewGameView.MapPreview.SetGeneralMapMode();
        NewGameView.MapPreview.SetMapName(GameController.Instance.SessionManager.WorldData.Name);
    }

    private void SetWorldAsCurrent(WorldData worldData)
    {
        GameController.Instance.SessionManager.SetWorldData(worldData);
    }

    public void SaveCurrentWorld()
    {
        GameController.Instance.SessionManager.SaveWorldData();
    }

    public void UpdateSaveFileNames()
    {
        WorldSaveFilenames = GameController.Instance.SessionManager.GetWorldSaveFiles();
    }

    public void LoadWorld(string worldName)
    {
        GameController.Instance.SessionManager.LoadWorldData(worldName);

        PreviewCurrentWorld();
    }

    public void GoToSimulation()
    {
        _mainMenuSceneController.SwitchMenu((int)MainMenuSceneSubMenus.Simulation);
    }
}
