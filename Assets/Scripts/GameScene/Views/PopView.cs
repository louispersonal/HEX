using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopView : PawnView
{
    [SerializeField] private LineRenderer _pathRenderer;
    
    public Pop Data;
    
    public void Initialize(Pop data)
    {
        Data = data;
        gameObject.transform.position = HexGridGeometry.AxialToScene(Data.Location);
    }

    public void MoveTo(AxialCoordinate newPosition)
    {
        gameObject.transform.position = HexGridGeometry.AxialToScene(newPosition);
    }
    
    public void Terminate()
    {
        
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (Data.CurrentJob is MoveJob)
        {
            _pathRenderer.gameObject.SetActive(true);
            DrawPath();
        }
    }

    public override void OnDeselected()
    {
        base.OnDeselected();
        _pathRenderer.gameObject.SetActive(false);
    }

    private void DrawPath()
    {
        MoveJob moveJob = Data.CurrentJob as MoveJob;
        if (moveJob == null) return;
        
        var path = moveJob.Path;
        Vector3[] pathPoints = new Vector3[path.Steps.Count - moveJob.StepIndex];

        int p = 0;
        for (int s = moveJob.StepIndex; s < path.Steps.Count; s++)
        {
            pathPoints[p] =  HexGridGeometry.AxialToScene(path.Steps[s].To);
            p++;
        }
        
        _pathRenderer.positionCount = pathPoints.Length;
        _pathRenderer.SetPositions(pathPoints);
    }
}
