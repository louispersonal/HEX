using UnityEngine;

public abstract class Assignment
{
    public abstract string AssignmentName { get; }
    public abstract Color Color { get; }
    public int Workers { get; private set; }
    
    protected Assignment(int workerCount)
    {
        Workers = workerCount;
    }

    public void AddWorkers(int addCount)
    {
        if (addCount < 0) Debug.LogError("Cannot add negative workers");
        Workers += addCount;
    }
    
    public void RemoveWorkers(int removeCount)
    {
        if (removeCount < 0) Debug.LogError("Cannot remove negative workers");
        removeCount = Mathf.Min(removeCount, Workers);
        Workers -= removeCount;
    }

    public abstract void Tick(Pop pop);

    public override string ToString()
    {
        return $"{AssignmentName}: {Workers}";
    }
}
