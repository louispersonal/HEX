using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job
{
    public JobStatus Status{ get; protected set; } = JobStatus.NotStarted;

    public bool IsComplete =>
        Status == JobStatus.Succeeded
        || Status == JobStatus.Failed
        || Status == JobStatus.Cancelled;

    private Pawn _pawn;
    
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