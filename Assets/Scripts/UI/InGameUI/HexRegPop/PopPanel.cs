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
    [SerializeField] private TextMeshProUGUI Assignments;
    [SerializeField] private ResourceView ResourceView;
    
    public void Populate(Pop pop)
    {
        Name.text = $"{pop.Name}";
        Population.text = $"{pop.Population}";
        Faction.text = $"{pop.Faction}";
        Culture.text = $"{pop.CultureID}";
        Religion.text = $"{pop.ReligionID}";
        Assignments.text = FormatAssignmentText(pop.Assignments);
        ResourceView.Populate(pop);
    }

    private string FormatAssignmentText(List<Assignment> assignments)
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
