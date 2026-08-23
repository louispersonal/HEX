public abstract class Assignment
{
    public int Workers { get; private set; }
    
    protected Assignment(int workerCount)
    {
        Workers = workerCount;
    }

    public void AddWorkers(int addCount)
    {
        Workers += addCount;
    }
    
    public void RemoveWorkers(int removeCount)
    {
        Workers -= removeCount;
    }

    public abstract void Tick(Pop pop);
}
