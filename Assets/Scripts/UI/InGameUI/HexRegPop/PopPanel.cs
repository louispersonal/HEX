using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopPanel : Panel
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
    
    private Pop _selectedPop;
    
    public void UpdatePanel()
    {
        Name.text = $"{_selectedPop.Name}";
        Population.text = $"{_selectedPop.Population}";
        Faction.text = $"{_selectedPop.Faction}";
        Culture.text = $"{_selectedPop.Culture.Name}";
        Religion.text = $"{_selectedPop.Religion.Name}";

        WedgeData[] pieChartData = new WedgeData[_selectedPop.Assignments.Count + 1];
        for (int i = 0; i < _selectedPop.Assignments.Count + 1; i++)
        {
            if (i < _selectedPop.Assignments.Count)
            {
                WedgeData data = new WedgeData();
                data.Color = _selectedPop.Assignments[i].Color;
                data.Label = _selectedPop.Assignments[i].AssignmentName;
                float value = _selectedPop.Assignments[i].Workers / (float)_selectedPop.Population;
                data.Value = value;
                pieChartData[i] = data;
            }
            else
            {
                WedgeData data = new WedgeData();
                data.Color = Color.grey;
                data.Label = "Special";
                float value = (_selectedPop.Population - _selectedPop.WorkingPopulation) / (float)_selectedPop.Population;
                data.Value = value;
                pieChartData[i] = data;
            }
        }
        
        AssignmentChart.BuildChart(pieChartData);
        
        ResourceView.Populate(_selectedPop.Stockpile.GetPreview());

        HidePlayerView();
        
        if (_selectedPop.ControlType == ControlType.Player)
        {
            ShowPlayerView();
        }
    }

    public void Initialize(Pop pop)
    {
        _selectedPop = pop;
        Initialized = true;
        UpdatePanel();
    }

    public void Terminate()
    {
        Initialized = false;
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
}
