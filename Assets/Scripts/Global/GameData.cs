using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData
{
    public string GameName;
    public string WorldName;
    
    public Ticker Ticker;
    
    public PopCollection Pops { get; } = new();
    public Dictionary<CultureID,  Culture> Cultures = new ();
    public Dictionary<ReligionID, Religion> Religions = new ();

    public GameSaveData ToSaveData()
    {
        return new GameSaveData(GameName, WorldName, Ticker.TickInfo,
            Pops.ToArray(), Cultures.Values.ToArray(), Religions.Values.ToArray());
    }
}

[System.Serializable]
public class GameSaveData
{
    public string GameName;
    public string WorldName;
    public TickInfo TickInfo;
    public Pop[] Pops;
    public Culture[] Cultures;
    public Religion[] Religions;

    public GameSaveData(string gameName, string worldName, TickInfo tickInfo,
        Pop[] pops, Culture[] cultures, Religion[] religions)
    {
        GameName = gameName;
        WorldName = worldName;
        TickInfo = tickInfo;
        Pops = pops;
        Cultures = cultures;
        Religions = religions;
    }
  }