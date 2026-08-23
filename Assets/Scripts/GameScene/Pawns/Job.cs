using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job
{
    public JobStatus Status{ get; protected set; }

    public JobType Type{ get; protected set; }
    
    public bool IsComplete =>
        Status == JobStatus.Succeeded
        || Status == JobStatus.Failed
        || Status == JobStatus.Cancelled;

    private int _ticksToComplete;
    private int _currentTick;
    
    protected Pawn _pawn;

    public Job(int ticksToComplete, Pawn pawn)
    {
        _ticksToComplete = ticksToComplete;
        _pawn = pawn;
        Status = JobStatus.NotStarted;
        _currentTick = 0;
    }
    
    public virtual void Start()
    {
        Status = JobStatus.InProgress;
    }

    public virtual void Progress(TickInfo tickInfo)
    {
        _currentTick++;
        if (_currentTick >= _ticksToComplete) Complete();
    }

    public virtual void Cancel()
    {
        Status = JobStatus.Cancelled;
    }

    protected virtual void Complete()
    {
        Status = JobStatus.Succeeded;
    }
}

public enum JobStatus
{
    NotStarted,
    InProgress,
    Succeeded,
    Failed,
    Cancelled
}

public enum JobType
{
    Parallel,
    Exclusive
}