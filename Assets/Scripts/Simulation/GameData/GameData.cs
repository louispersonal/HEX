using System;
using System.Collections.Generic;

public class GameData
{
    public string GameName { get; }
    public string WorldName { get; }
    
    public Ticker Ticker;
    
    public PopCollection Pops { get; } = new();
    public IndexedDictionary<CultureID,  Culture> Cultures = new ();
    public IndexedDictionary<ReligionID, Religion> Religions = new ();
    public IndexedDictionary<FactionID, Faction> Factions = new();

    public GameData(string gameName, string worldName)
    {
        GameName = gameName;
        WorldName = worldName;
        Ticker = new Ticker(new TickInfo());
    }

    public GameData(GameSaveData saveData) : this(saveData.GameName, saveData.WorldName)
    {
        Ticker = new Ticker(saveData.TickInfo);

        Pops.LoadPops(saveData.Pops);

        foreach (Religion religion in saveData.Religions)
        {
            if (!Religions.TryAdd(religion))
                throw new InvalidOperationException($"Duplicate Religion ID: {religion.ID.Value}");
        }

        foreach (Culture culture in saveData.Cultures)
        {
            if (!Cultures.TryAdd(culture))
                throw new  InvalidOperationException($"Duplicate Culture ID: {culture.ID.Value}");
        }

        foreach (Faction faction in saveData.Factions)
        {
            if (!Factions.TryAdd(faction))
                throw new  InvalidOperationException($"Duplicate Faction ID: {faction.ID.Value}");
        }
    }
    
    public GameSaveData ToSaveData()
    {
        return new GameSaveData(GameName, WorldName, Ticker.TickInfo,
            Pops.ToSaveDataArray(), Cultures.ToArray(), Religions.ToArray(), Factions.ToArray());
    }

    public IEnumerable<ITickable> GetTickables()
    {
        foreach (Pop pop in Pops.Pops) yield return pop;
    }
}

[System.Serializable]
public class GameSaveData
{
    public string GameName;
    public string WorldName;
    public TickInfo TickInfo;
    public PopSaveData[] Pops;
    public Culture[] Cultures;
    public Religion[] Religions;
    public Faction[] Factions;

    public GameSaveData(string gameName, string worldName, TickInfo tickInfo,
        PopSaveData[] pops, Culture[] cultures, Religion[] religions, Faction[] factions)
    {
        GameName = gameName;
        WorldName = worldName;
        TickInfo = tickInfo;
        Pops = pops;
        Cultures = cultures;
        Religions = religions;
        Factions = factions;
    }
  }