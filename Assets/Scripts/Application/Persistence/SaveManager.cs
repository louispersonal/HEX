using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveManager
{
    private string WorldsFolder => Path.Combine(Application.persistentDataPath, "Worlds");

    private string GamesFolder => Path.Combine(Application.persistentDataPath, "Games");
    
    private SessionManager _sessionManager;

    public SaveManager(SessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }
    
    public List<string> GetWorldSaveFiles()
    {
        List<string> saveFiles = new List<string>();
        if (Directory.Exists(WorldsFolder))
        {
            string[] files = Directory.GetFiles(WorldsFolder, "*.json");

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                saveFiles.Add(fileName);
            }
        }

        return saveFiles;
    }

    public List<string> GetGameSaveFiles()
    {
        List<string> saveFiles = new List<string>();
        if (Directory.Exists(GamesFolder))
        {
            string[] files = Directory.GetFiles(GamesFolder, "*.json");

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                saveFiles.Add(fileName);
            }
        }

        return saveFiles;
    }
    
    public void SaveWorldData(WorldData worldData)
    {
        Directory.CreateDirectory(WorldsFolder);
        
        WorldSaveData saveData = worldData.ToSaveData();
        string json = JsonUtility.ToJson(saveData, true);
        string path = WorldsFolder + "/" + saveData.WorldName + ".json";
        File.WriteAllText(path, json);
    }

    public WorldData LoadWorldData(string worldName)
    {
        string json = File.ReadAllText(GetWorldPathFromName(worldName));
        WorldSaveData saveData = JsonUtility.FromJson<WorldSaveData>(json);
        WorldData loadedWorld = new WorldData(saveData.Hexes);
        
        foreach (River river in saveData.Rivers)
        {
            river.PopulateRiverConnections();
        }
        
        loadedWorld.Rivers = new SpatialLookup<RiverID, River>(saveData.Rivers);
        loadedWorld.Lakes = new SpatialLookup<LakeID, Lake>(saveData.Lakes);
        loadedWorld.GeoFeatures = new SpatialLookup<GeoID, GeoFeature>(saveData.GeoFeatures);

        loadedWorld.Regions = new IndexedDictionary<RegionID, Region>(saveData.Regions);
        
        return loadedWorld;
    }

    private string GetWorldPathFromName(string worldName)
    {
        return Path.Combine(WorldsFolder, worldName + ".json");
    }

    private void SaveGameData(GameData gameData)
    {
        Directory.CreateDirectory(GamesFolder);
        
        GameSaveData saveData = gameData.ToSaveData();
        string json = JsonUtility.ToJson(saveData, true);
        string path = GamesFolder + "/" + saveData.GameName + ".json";
        File.WriteAllText(path, json);
    }

    private GameData LoadGameData(string gameName)
    {
        string json = File.ReadAllText(GetGamePathFromName(gameName));
        GameSaveData saveData =  JsonUtility.FromJson<GameSaveData>(json);

        GameData gameData = new GameData();
        gameData.GameName = saveData.GameName;
        gameData.WorldName = saveData.WorldName;
        gameData.Ticker = new Ticker(saveData.TickInfo);
        gameData.Pops.LoadPops(saveData.Pops);
        
        foreach(Religion religion in saveData.Religions)
        {
            gameData.Religions.Add(religion.ID, religion);
        }
        
        foreach(Culture culture in saveData.Cultures)
        {
            gameData.Cultures.Add(culture.ID, culture);
        }

        return gameData;
    }

    private string GetGamePathFromName(string gameName)
    {
        return Path.Combine(GamesFolder, gameName + ".json");
    }
    
    public void LoadGame(string gameName)
    {
        GameData gameData = LoadGameData(gameName);
        
        string worldName = gameData.WorldName;
        
        WorldData worldData = LoadWorldData(worldName);
        
        _sessionManager.SetSession(worldData, gameData);
    }

    public void SaveGame()
    {
        string gameName = _sessionManager.GameData.GameName;
        string worldName = _sessionManager.GameData.WorldName;
        
        SaveWorldData(_sessionManager.WorldData);
        SaveGameData(_sessionManager.GameData);
    }
}
