using System;
using System.Diagnostics;

public static class AssignmentCreator
{
    public static Assignment CreateAssignment(int workers, AssignmentID assignmentID)
    {
        switch (assignmentID.Value)
        {
            case (int) AssignmentTypes.Gather:
                return new GatherAssignment(workers);
            case (int) AssignmentTypes.Hunt:
                return new HuntAssignment(workers);
            default:
                throw new ArgumentException("Invalid assignment ID", nameof(assignmentID)); 
        }
    }
}

public enum AssignmentTypes
{
    Gather = 0,
    Hunt = 1
}