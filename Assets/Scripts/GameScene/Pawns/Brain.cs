using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : ITickable
{
    private readonly Pawn _pawn;
    public Pawn Pawn => _pawn;

    private Queue<Job> _jobs = new();
    
    public Brain(Pawn pawn)
    {
        _pawn = pawn;
    }

    public void Tick(TickInfo tickInfo)
    {
        ProgressJob(tickInfo);
    }

    public void EnqueueJob(Job job)
    {
        _jobs.Enqueue(job);
    }
    
    private void ProgressJob(TickInfo tickInfo)
    {
        if (_jobs.Count == 0) return;

        Job currentJob = _jobs.Peek();
        
        if (currentJob.Status == JobStatus.NotStarted) currentJob.Start(_pawn);
        
        currentJob.Progress(tickInfo);
        
        if (currentJob.IsComplete)
        {
            _jobs.Dequeue();
        }
    }
}
