using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITickable
{
    public int Order { get; }
    public void Tick(TickInfo tickInfo);
}
