using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITickable { }

public interface IDecisionTick : ITickable
{
    void DecisionTick(TickInfo tickInfo);
}

public interface IAssignmentTick : ITickable
{
    void AssignmentTick(TickInfo tickInfo);
}

public interface IResolutionTick : ITickable
{
    void ResolutionTick(TickInfo tickInfo);
}

public interface IUpkeepTick : ITickable
{
    void UpkeepTick(TickInfo tickInfo);
}

public interface IUITickable : ITickable
{
    void UITick(TickInfo tickInfo);
}