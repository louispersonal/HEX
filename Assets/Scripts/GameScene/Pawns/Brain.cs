using System.Collections.Generic;

public class Brain : IDecisionTick
{
    public int Order => 0;
    
    private readonly Pawn _pawn;
    public Pawn Pawn => _pawn;

    private List<Job> _jobs = new();
    
    public Brain(Pawn pawn)
    {
        _pawn = pawn;
    }
    
    public virtual void DecisionTick(TickInfo tickInfo)
    {
        ProgressJobs(tickInfo);
    }

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
}
