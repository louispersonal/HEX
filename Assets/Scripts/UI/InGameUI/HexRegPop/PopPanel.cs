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
    
    public void Populate(Pop pop)
    {
        Name.text = $"{pop.Name}";
        Population.text = $"{pop.Population}";
        Faction.text = $"{pop.Faction}";
        Culture.text = $"{pop.Culture.Name}";
        Religion.text = $"{pop.Religion.Name}";

        WedgeData[] pieChartData = new WedgeData[pop.Assignments.Count];
        for (int i = 0; i < pop.Assignments.Count; i++)
        {
            WedgeData data = new WedgeData();
            data.Color = pop.Assignments[i].Color;
            data.Label = pop.Assignments[i].AssignmentName;
            float value = pop.Assignments[i].Workers / (float)pop.Population;
            data.Value = value;
            pieChartData[i] = data;
        }
        
        AssignmentChart.BuildChart(pieChartData);
        
        Assignments.text = FormatAssignmentText(pop.Assignments);
        ResourceView.Populate(pop.Stockpile.GetPreview());
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
}
