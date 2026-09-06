using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PieChart : MonoBehaviour
{
    [SerializeField] private PieWedge _pieWedgePrefab;

    private WedgeData[] _data;
    
    private List<PieWedge> _activeWedges = new ();
    
    public void BuildChart(WedgeData[] data)
    {
        ClearActiveWedges();
        
        _data = data;
        float amountFilled = 0f;
        foreach (var wedge in _data)
        {
            PieWedge currentWedge = Instantiate(_pieWedgePrefab, gameObject.transform);
            currentWedge.SetColor(wedge.Color);
            currentWedge.SetFill(wedge.Value);
            currentWedge.SetAngle(360f * amountFilled);
            _activeWedges.Add(currentWedge);
            amountFilled += wedge.Value;
        }
    }
    
    private void ClearActiveWedges()
    {
        foreach (var wedge in _activeWedges)
        {
            Destroy(wedge.gameObject);
        }
        _activeWedges.Clear();
    }
}

public class WedgeData
{
    public string Label;
    public Color Color;
    public float Value;
}