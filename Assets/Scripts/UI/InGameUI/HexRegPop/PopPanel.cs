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
    [SerializeField] private TextMeshProUGUI Assignments;
    [SerializeField] private ResourceView ResourceView;
    
    private Pop _selectedPop;
    
    public void UpdatePanel()
    {
        Name.text = $"{_selectedPop.Name}";
        Population.text = $"{_selectedPop.Population}";
        Faction.text = $"{_selectedPop.Faction}";
        Culture.text = $"{_selectedPop.Culture.Name}";
        Religion.text = $"{_selectedPop.Religion.Name}";

        WedgeData[] pieChartData = new WedgeData[_selectedPop.Assignments.Count];
        for (int i = 0; i < _selectedPop.Assignments.Count; i++)
        {
            WedgeData data = new WedgeData();
            data.Color = _selectedPop.Assignments[i].Color;
            data.Label = _selectedPop.Assignments[i].AssignmentName;
            float value = _selectedPop.Assignments[i].Workers / (float)_selectedPop.Population;
            data.Value = value;
            pieChartData[i] = data;
        }
        
        AssignmentChart.BuildChart(pieChartData);
        
        Assignments.text = FormatAssignmentText(_selectedPop.Assignments);
        ResourceView.Populate(_selectedPop.Stockpile.GetPreview());
    }

    public void Initialize(Pop pop)
    {
        _selectedPop = pop;
        Initialized = true;
        UpdatePanel();
    }

    private string FormatAssignmentText(IReadOnlyList<Assignment> assignments)
    {
        string text = "";
        for (int i = 0; i < assignments.Count; i++)
        {
            text += assignments[i].ToString();
            if (i != assignments.Count - 1) text += "\n";
        }

        return text;
    }

    public void Terminate()
    {
        
        Initialized = false;
    }
}
