using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopView : PawnView
{
    [SerializeField] private LineRenderer _pathRenderer;
    
    public Pop Pop => (Pop)Data;
    
    public void Initialize(Pop data)
    {
        InitializePawn(data);
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
        if (Data.CurrentJob is not MoveJob moveJob || moveJob.Path == null)
        {
            _pathRenderer.gameObject.SetActive(false);
            return;
        }

        var path = moveJob.Path;

        int remainingSteps = path.Steps.Count - moveJob.StepIndex;

        Vector3[] points = new Vector3[remainingSteps + 1];

        points[0] = HexGridGeometry.AxialToScene(Data.Location);

        for (int i = 0; i < remainingSteps; i++)
        {
            Vector3 zOffset = new Vector3(0, 0, -0.01f);
            points[i + 1] = (Vector3)HexGridGeometry.AxialToScene(path.Steps[moveJob.StepIndex + i].To) + zOffset;
        }

        _pathRenderer.positionCount = points.Length;
        _pathRenderer.SetPositions(points);
        _pathRenderer.gameObject.SetActive(true);
    }
}
