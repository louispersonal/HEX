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

        Color[] pixels = TextureUtilities.GetPixelsFromWorldData(GameController.Instance.SessionManager.WorldData, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]);
        NewGameView.UpdatePreview(TextureUtilities.GetTexture(pixels, NewGameView.PreviewImageResolution[0], NewGameView.PreviewImageResolution[1]));
    }

    private void SetWorldAsCurrent(WorldData worldData)
    {
        GameController.Instance.SessionManager.SetWorldData(worldData);
    }
}
