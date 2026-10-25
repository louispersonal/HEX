using System;
using UnityEngine;

[System.Serializable]
public abstract class Assignment : ISavable<AssignmentData>
{
    public AssignmentID AssignmentID { get; }
    public abstract string AssignmentName { get; }
    public abstract Color Color { get; }
    public int Workers { get; private set; }

    protected Assignment(AssignmentData assignmentData)
    {
        Workers = assignmentData.Workers;
    }
    
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

    public AssignmentData ToSaveData()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return $"{AssignmentName}: {Workers}";
    }
}

public class AssignmentData
{
    public AssignmentID ID;
    public int Workers;
    public AssignmentData(Assignment assignment)
    {
        ID = assignment.AssignmentID;
        Workers = assignment.Workers;
    }
}

[Serializable]
public class AssignmentID : IEquatable<AssignmentID>
{
    public readonly int Value;
    public AssignmentID(int value) =>  Value = value;

    public bool Equals(AssignmentID other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((AssignmentID)obj);
    }

    public override int GetHashCode()
    {
        return Value;
    }
}