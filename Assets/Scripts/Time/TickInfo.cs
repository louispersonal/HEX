using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickInfo
{
    public long TickCount { get; private set; }

    public TickInfo()
    {
        TickCount = 0L;
    }
    
    public long Increment()
    {
        TickCount++;
        return TickCount;
    }
}
