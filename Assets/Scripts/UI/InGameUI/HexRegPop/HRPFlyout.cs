using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRPFlyout : Flyout
{
    public HexPanel HexPanel => _panels[0] as HexPanel;
    public RegionPanel RegionPanel => _panels[1] as RegionPanel;
    public PopPanel PopPanel => _panels[2] as PopPanel;
    
    private SelectionManager SelectionManager => GameSceneController.Instance.SelectionManager;
    
    LocationSelection _locationSelection;

    private void Start()
    {
        SelectionManager.OnLocationSelectionChanged += OnLocationSelectionChanged;
        SelectionManager.OnLocationDeselected += OnLocationDeselected;
    }

    private void OnLocationSelectionChanged(LocationSelection locationSelection)
    {
        _locationSelection = locationSelection;
        
        if (!IsOpen) OpenFlyOut();
        
        UpdateFlyout();
    }

    private void OnLocationDeselected()
    {
        _locationSelection = null;
        base.CloseFlyOut();
    }
    
    protected override void UpdateFlyout()
    {
        HexPanel.SetData(_locationSelection.SelectedHex);
        HexPanel.UpdatePanel();
        
        RegionPanel.SetData(_locationSelection.SelectedRegion);
        RegionPanel.UpdatePanel();
        
        PopPanel.SetData(_locationSelection.SelectedPop);
        PopPanel.UpdatePanel();
    }
}
