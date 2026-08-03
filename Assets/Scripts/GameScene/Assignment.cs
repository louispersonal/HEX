public abstract class Assignment
{
    public int Workers { get; private set; }
    
    protected Assignment(int workerCount)
    {
        Workers = workerCount;
    }

    public void ChangeWorkerCount(int newCount)
    {
        Workers = newCount;
    }

    public abstract void Tick(Pop pop);
}
