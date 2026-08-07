using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITickable
{
    public TickableType TickableType { get; }
    public void Tick(TickInfo tickInfo);
}
