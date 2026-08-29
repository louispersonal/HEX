using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieChart : MonoBehaviour
{
    [SerializeField] private PieWedge _pieWedgePrefab;

    public void BuildChart(WedgeData[] data)
    {
        float amountFilled = 0f;
        foreach (var wedge in data)
        {
            PieWedge currentWedge = Instantiate(_pieWedgePrefab, gameObject.transform);
            currentWedge.SetColor(wedge.Color);
            currentWedge.SetFill(wedge.Value);
            currentWedge.SetAngle(360f * amountFilled);
            amountFilled += wedge.Value;
        }
    }
}

public class WedgeData
{
    public string Label;
    public Color Color;
    public float Value;
}