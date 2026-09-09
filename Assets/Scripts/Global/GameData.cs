using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Ticker Ticker;
    
    public PopCollection Pops { get; } = new();
    public Dictionary<CultureID,  Culture> Cultures = new ();
    public Dictionary<ReligionID, Religion> Religions = new ();
}
