using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJob : Job
{
    private Pop _pop;

    private AxialCoordinate _destination;
    
    private Pathfinder Pathfinder => GameController.Instance.SessionManager.WorldData.Pathfinder;

    private AStarPath _path;

    private int _stepIndex;

    private int _ticksOnCurrentStep;
    
    public MoveJob(Pop pop, AxialCoordinate destination) : base(pop)
    {
        _pop = pop;
        _destination = destination;
        Type = JobType.Exclusive;
    }

    public override void Progress(TickInfo tickInfo)
    {
        UpdateStep();
        base.Progress(tickInfo);
    }

    protected override void SetTicksToComplete()
    {
        var costProvider = new PopMovementCostProvider(_pop);
        _path = Pathfinder.AStar(_pop.Location, _destination, costProvider);
        _stepIndex = 0;
        _ticksOnCurrentStep = 0;

        if (_path == null)
        {
            Status = JobStatus.Failed;
            return;
        }

        _ticksToComplete = Mathf.CeilToInt(_path.TotalCost);

        if (_path.Steps.Count == 0) Complete();
    }

    private void UpdateStep()
    {
        AStarPathStep currentStep = _path.Steps[_stepIndex];

        _ticksOnCurrentStep++;

        if (_ticksOnCurrentStep < currentStep.Cost)
        {
            return;
        }

        GameController.Instance.SessionManager.GameData.Pops.TryMove(_pop, currentStep.To);
        _stepIndex++;
        _ticksOnCurrentStep = 0;
    }
}
