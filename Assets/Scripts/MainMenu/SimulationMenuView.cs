using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationMenuView : SubMenuView
{
    [SerializeField] private TMP_InputField _simulationLengthField;
    [SerializeField] private RawImage _previewImage;
    [SerializeField] private TextMeshProUGUI _previewName;
    [SerializeField] private LoadingPanel _loadingPanel;
    
    [SerializeField] private RectTransform _popViewPrefab;
    
    private int _defaultSimulationLength = 10000;
    private int _simulationLengthYears;
    private RectTransform _mapRect;
    private Dictionary<Pop, RectTransform> _popIcons = new();
    
    SimulationMenuController SimulationMenuController { get { return _subMenu as SimulationMenuController; } }
    
    protected override void Start()
    {
        base.Start();
        _simulationLengthYears = _defaultSimulationLength;
        _simulationLengthField.text = _simulationLengthYears.ToString();
        _mapRect = _previewImage.rectTransform;
    }

    private void OnEnable()
    {
        InitializePreview();
    }

    public void UpdateSimulationLength(string value)
    {
        _simulationLengthYears = int.Parse(value);
    }

    public void StartSimulation()
    {
        SimulationMenuController.Simulate(_simulationLengthYears, _loadingPanel, UpdatePreview);
    }

    public void StartGame()
    {
        SimulationMenuController.StartGame();
    }

    private void InitializePreview()
    {
        _previewName.text = GameController.Instance.SessionManager.WorldData.Name;
        _previewImage.texture = GameController.Instance.SessionManager.UiData.MiniMapTexture.Texture;
    }

    private void UpdatePreview()
    {
        foreach (var pop in GameController.Instance.SessionManager.GameData.Pops.Values)
        {
            if (!_popIcons.ContainsKey(pop)) _popIcons.Add(pop, Instantiate(_popViewPrefab, transform));
            _popIcons[pop].gameObject.SetActive(true);
            _popIcons[pop].position = GetPopIconWorldPosition(pop.Location);
        }
    }
    
    private Vector3 GetPopIconWorldPosition(AxialCoordinate popAxial)
    {
        Vector2 mapPixelPosition = AxialGeometry.AxialToCartesian(
            popAxial,
            GameController.Instance.SessionManager.UiData.MiniMapTexture.HexSizePixels);

        float uiScaleX = _mapRect.rect.width 
                         / GameController.Instance.SessionManager.UiData.MiniMapTexture.Texture.width;
        float uiScaleY = _mapRect.rect.height 
                         / GameController.Instance.SessionManager.UiData.MiniMapTexture.Texture.height;

        Vector2 localPosition = new Vector2(
            mapPixelPosition.x * uiScaleX,
            mapPixelPosition.y * uiScaleY);

        Vector3[] corners = new Vector3[4];
        _mapRect.GetWorldCorners(corners);

        return corners[1] + (Vector3)localPosition;
    }
}
