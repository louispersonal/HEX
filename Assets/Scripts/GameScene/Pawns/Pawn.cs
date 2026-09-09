using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : IJobTickable, IUpkeepTick
{
    public Brain Brain { get; private set; }

    public ControlType ControlType { get; private set; } = ControlType.Player;

    public void SetBrain(Brain brain)
    {
        if (Brain != null)
        {
            throw new System.InvalidOperationException("Pawn already has a brain.");
        }
        ControlType = ControlType.AI;
        Brain = brain;
    }
    
    private List<Job> _jobs = new();
    
    public void AddJob(Job job)
    {
        _jobs.Add(job);
    }
    
    private void ProgressJobs(TickInfo tickInfo)
    {
        for (int i = 0; i < _jobs.Count; i++)
        {
            Job job = _jobs[i];

            ProgressJob(job, tickInfo);

            if (job.Type == JobType.Exclusive)
            {
                break;
            }
        }

        _jobs.RemoveAll(job => job.IsComplete);
    }

    private void ProgressJob(Job job, TickInfo tickInfo)
    {
        if (job.Status == JobStatus.NotStarted)
        {
            job.Start();
        }

        if (!job.IsComplete)
        {
            job.Progress(tickInfo);
        }
    }
    
    public virtual void JobTick(TickInfo tickInfo)
    {
        ProgressJobs(tickInfo);
    }
    
    public virtual void UpkeepTick(TickInfo tickInfo)
    {
        
    }
}

public enum ControlType
{
    Player,
    AI
}