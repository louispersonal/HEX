using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJob : Job
{
    private Pop _pop => _pawn as Pop;

    private AxialCoordinate _destination;
    
    public MoveJob(int ticksToComplete, Pop pop, AxialCoordinate destination) : base(ticksToComplete, pop)
    {
        _destination = destination;
    }
    
    protected override void Complete()
    {
        _pop.Location = _destination;
        base.Complete();
    }
}
