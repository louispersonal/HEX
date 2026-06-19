using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewGameMenuView : SubMenuView
{
    [SerializeField] private TMP_InputField _seedField;
    [SerializeField] private TMP_InputField _widthField;
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private MapPreviewController _mapPreview;
    [SerializeField] private TMP_Dropdown _loadDropdown;

    public MapPreviewController MapPreview { get { return _mapPreview; } }

    public LoadingPanel WorldGenLoadingPanel;

    NewGameMenuController NewGameMenu { get { return _subMenu as NewGameMenuController; } }

    private int _seedFieldValue;

    private int _widthFieldValue;

    private string _nameFieldValue;

    private int _defaultSeedValue = 2332;

    private int _defaultWdithValue = 150;

    public int[] PreviewImageResolution = { 1200, 695 }; // width, height

    protected override void Start()
    {
        base.Start();

        _seedFieldValue = _defaultSeedValue;
        _seedField.text = _seedFieldValue.ToString();
        _widthFieldValue = _defaultWdithValue;
        _widthField.text = _widthFieldValue.ToString();
        UpdateSaveFilesDropdown();
    }

    public void UpdateSeed(string value)
    {
        _seedFieldValue = int.Parse(value);
    }

    public void UpdateWidth(string value)
    {
        _widthFieldValue = int.Parse(value);
    }

    public void UpdateName(string value)
    {
        _nameFieldValue = value;
    }

    public void GenerateButton()
    {
        NewGameMenu.GenerateWorld(_widthFieldValue, _seedFieldValue, _nameFieldValue);
    }

    public void SaveButton()
    {
        NewGameMenu.SaveCurrentWorld();
    }

    public void LoadButton()
    {
        NewGameMenu.LoadWorld(_loadDropdown.options[_loadDropdown.value].text);
    }

    public void UpdateSaveFilesDropdown()
    {
        NewGameMenu.UpdateSaveFileNames();
        _loadDropdown.ClearOptions();
        _loadDropdown.AddOptions(NewGameMenu.WorldSaveFilenames);
    }

    public void StartGameButton()
    {
        NewGameMenu.StartGame();
    }
}
