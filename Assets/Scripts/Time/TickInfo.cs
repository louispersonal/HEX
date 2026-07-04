using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickInfo
{
    public long TickCount { get; private set; }

    public long Increment()
    {
        TickCount++;
        return TickCount;
    }
}
