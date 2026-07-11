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
    
    private int _defaultSimulationLength = 10000;
    private int _simulationLengthYears;

    SimulationMenuController SimulationMenuController { get { return _subMenu as SimulationMenuController; } }
    
    protected override void Start()
    {
        base.Start();
        _simulationLengthYears = _defaultSimulationLength;
        _simulationLengthField.text = _simulationLengthYears.ToString();
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
        SimulationMenuController.Simulate(_simulationLengthYears, _loadingPanel);
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
}
