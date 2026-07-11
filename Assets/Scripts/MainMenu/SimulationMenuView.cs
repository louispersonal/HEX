using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimulationMenuView : SubMenuView
{
    [SerializeField] private TMP_InputField _simulationLengthField;
    private int _defaultSimulationLength = 10000;
    private int _simulationLength;

    SimulationMenuController SimulationMenuController { get { return _subMenu as SimulationMenuController; } }
    
    protected override void Start()
    {
        base.Start();
        _simulationLength = _defaultSimulationLength;
        _simulationLengthField.text = _simulationLength.ToString();
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
}
