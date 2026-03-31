using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewGameMenuView : SubMenuView
{
    [SerializeField] private TMP_InputField _seedField;
    [SerializeField] private TMP_InputField _widthField;
    [SerializeField] private UnityEngine.UI.RawImage _rawImageHexPreview;

    NewGameMenuController NewGameMenu { get { return _subMenu as NewGameMenuController; } }

    private int _seedFieldValue;

    private int _widthFieldValue;

    private int _defaultSeedValue = 2332;

    private int _defaultWdithValue = 150;

    private int[] _previewImageResolution = { 1200, 695 }; // width, height

    protected override void Start()
    {
        base.Start();

        _seedFieldValue = _defaultSeedValue;
        _seedField.text = _seedFieldValue.ToString();
        _widthFieldValue = _defaultWdithValue;
        _widthField.text = _widthFieldValue.ToString();
    }

    public void UpdateSeed(string value)
    {
        _seedFieldValue = int.Parse(value);
    }

    public void UpdateWidth(string value)
    {
        _widthFieldValue = int.Parse(value);
    }

    public void GenerateButton()
    {
        StartCoroutine(GenerateWorldAndPreview());
    }

    private IEnumerator GenerateWorldAndPreview()
    {
        Debug.Log("Generating world...");
        NewGameMenu.GenerateWorld(_widthFieldValue, _seedFieldValue);
        while (GameController.Instance.SessionManager.WorldData == null)
        {
            yield return null;
        }
        Debug.Log("World Generation Finished");
        Debug.Log("Previewing world...");
        _rawImageHexPreview.texture = TextureUtilities.GetTexture
            (TextureUtilities.GetPixelsFromHexGrid(GameController.Instance.SessionManager.WorldData.Grid,
            _previewImageResolution[0], _previewImageResolution[1]), _previewImageResolution[0], _previewImageResolution[1]);
        Debug.Log("Preview finished");
    }
}
