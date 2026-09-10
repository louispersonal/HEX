using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopWindow : MonoBehaviour, IUITickable
{
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Population;
    [SerializeField] private TextMeshProUGUI Faction;
    [SerializeField] private TextMeshProUGUI Culture;
    [SerializeField] private TextMeshProUGUI Religion;
    [SerializeField] private PieChart AssignmentChart;
    [SerializeField] private ResourceView ResourceView;
    [SerializeField] private GameObject ActionPanel;
    [SerializeField] private GameObject ModifyAssignmentsButton;
    [SerializeField] private GameObject _content;

    private SelectionManager SelectionManager => GameSceneController.Instance.SelectionManager;
    
    public bool IsOpen;
    
    private Pop _popData;

    public void Open()
    {
        _content.SetActive(true);
        IsOpen = true;
    }

    public void Close()
    {
        _content.SetActive(false);
        IsOpen = false;
    }
    
    private void Start()
    {
        SelectionManager.OnPrimarySelectionChanged += OnPrimarySelectionChanged;
        SelectionManager.OnPrimaryDeselected += OnPrimaryDeselected;
    }
    
    private void OnPrimarySelectionChanged(Pawn before, Pawn current)
    {
        if (!IsOpen) Open();

        if ((current as Pop) != null)
        {
            _popData = (Pop) current;
            UpdateWindow();
        }
    }

    private void OnPrimaryDeselected()
    {
        Close();
    }
    
    public void UpdateWindow()
    {
        if (_popData == null)
        {
            Clear();
            return;
        }

        Name.text = $"{_popData.Name}";
        Population.text = $"{_popData.Population}";
        Faction.text = $"{_popData.Faction}";
        Culture.text = $"{_popData.Culture.Name}";
        Religion.text = $"{_popData.Religion.Name}";

        WedgeData[] pieChartData = new WedgeData[_popData.Assignments.Count + 1];
        for (int i = 0; i < _popData.Assignments.Count + 1; i++)
        {
            if (i < _popData.Assignments.Count)
            {
                WedgeData data = new WedgeData();
                data.Color = _popData.Assignments[i].Color;
                data.Label = _popData.Assignments[i].AssignmentName;
                float value = _popData.Assignments[i].Workers / (float)_popData.Population;
                data.Value = value;
                pieChartData[i] = data;
            }
            else
            {
                WedgeData data = new WedgeData();
                data.Color = Color.grey;
                data.Label = "Special";
                float value = (_popData.Population - _popData.WorkingPopulation) / (float)_popData.Population;
                data.Value = value;
                pieChartData[i] = data;
            }
        }
        
        AssignmentChart.BuildChart(pieChartData);
        
        ResourceView.Populate(_popData.Stockpile.GetPreview());

        HidePlayerView();
        
        if (_popData.ControlType == ControlType.Player)
        {
            ShowPlayerView();
        }
    }
    
    private void ShowPlayerView()
    {
        ActionPanel.SetActive(true);
        ModifyAssignmentsButton.SetActive(true);
    }

    private void HidePlayerView()
    {
        ActionPanel.SetActive(false);
        ModifyAssignmentsButton.SetActive(false);
    }

    private void Clear()
    {
        Name.text = "";
        Population.text = "";
        Faction.text = "";
        Culture.text = "";
        Religion.text = "";
        
        AssignmentChart.Clear();
        ResourceView.Clear();
        HidePlayerView();
    }

    public void UITick(TickInfo tickInfo)
    {
        if (!IsOpen) return;
        
        UpdateWindow();
    }
    
}
