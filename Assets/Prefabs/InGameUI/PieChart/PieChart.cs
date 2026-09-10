using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PieChart : MonoBehaviour
{
    [SerializeField] private PieWedge _pieWedgePrefab;
    [SerializeField] private Transform _chartTransform;
    [SerializeField] private TextMeshProUGUI _legend;

    private WedgeData[] _data;
    
    private List<PieWedge> _activeWedges = new ();
    
    public void BuildChart(WedgeData[] data)
    {
        Clear();
        
        _data = data;
        float amountFilled = 0f;
        foreach (var wedge in _data)
        {
            PieWedge currentWedge = Instantiate(_pieWedgePrefab, _chartTransform);
            currentWedge.SetColor(wedge.Color);
            currentWedge.SetFill(wedge.Value);
            currentWedge.SetAngle(-360f * amountFilled);
            _activeWedges.Add(currentWedge);
            amountFilled += wedge.Value;
        }
        
        SetLegend();
    }

    public void Clear()
    {
        ClearActiveWedges();
        _legend.text = "";
    }
    
    private void ClearActiveWedges()
    {
        foreach (var wedge in _activeWedges)
        {
            Destroy(wedge.gameObject);
        }
        _activeWedges.Clear();
    }

    private void SetLegend()
    {
        string block = "";
        foreach (var wedge in _data)
        {
            ;
            string line = $"<color=#{wedge.Color.ToHexString()}>■</color> - ";
            line += wedge.Label;
            line += "\n";
            block += line;
        }
        _legend.text = block;
    }
}

public class WedgeData
{
    public string Label;
    public Color Color;
    public float Value;
}