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
    
    private int _defaultSimulationLength = 10000;
    private int _simulationLength;

    SimulationMenuController SimulationMenuController { get { return _subMenu as SimulationMenuController; } }
    
    protected override void Start()
    {
        base.Start();
        _simulationLength = _defaultSimulationLength;
        _simulationLengthField.text = _simulationLength.ToString();
    }

    private void OnEnable()
    {
        InitializePreview();
    }

    public void UpdateSimulationLength(string value)
    {
        _simulationLength = int.Parse(value);
    }

    public void StartSimulation()
    {
        
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
