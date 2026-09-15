using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData
{
    private string GameName = "test_game";
    
    public Ticker Ticker;
    
    public PopCollection Pops { get; } = new();
    public Dictionary<CultureID,  Culture> Cultures = new ();
    public Dictionary<ReligionID, Religion> Religions = new ();

    public GameSaveData ToSaveData()
    {
        return new GameSaveData(GameName, Pops.ToArray(), Cultures.Values.ToArray(), Religions.Values.ToArray());
    }
}

[System.Serializable]
public class GameSaveData
{
    public string GameName;
    public Pop[] Pops;
    public Culture[] Cultures;
    public Religion[] Religions;

    public GameSaveData(string gameName, Pop[] pops, Culture[] cultures, Religion[] religions)
    {
        GameName = gameName;
        Pops = pops;
        Cultures = cultures;
        Religions = religions;
    }
  }