using System;
using System.Diagnostics;

public static class AssignmentCreator
{
    public static Assignment CreateAssignment(int workers, AssignmentID assignmentID)
    {
        switch (assignmentID.Value)
        {
            case 0:
                return new GatherAssignment(workers);
            case 1:
                return new HuntAssignment(workers);
            default:
                throw new ArgumentException("Invalid assignment ID", nameof(assignmentID)); 
        }
    }
}