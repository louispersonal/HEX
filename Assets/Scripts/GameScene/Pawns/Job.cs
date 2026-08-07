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
    
    private Pawn _pawn;

    public Job(int ticksToComplete)
    {
        _ticksToComplete = ticksToComplete;
        Status = JobStatus.NotStarted;
    }
    
    public virtual void Start(Pawn pawn)
    {
        _pawn = pawn;
        Status = JobStatus.InProgress;
    }

    public virtual void Progress(TickInfo tickInfo)
    {
        
    }

    public virtual void Cancel()
    {
        Status = JobStatus.Cancelled;
    }

    public virtual void Complete()
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