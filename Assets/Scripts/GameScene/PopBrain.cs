using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopBrain : ITickable
{   
    private readonly Pop _pop;
    public Pop Pop => _pop;

    public PopBrain(Pop pop)
    {
        _pop = pop;
    }
    public void Tick(TickInfo tickInfo)
    {
        // Pop.DoSomething()
    }
}
