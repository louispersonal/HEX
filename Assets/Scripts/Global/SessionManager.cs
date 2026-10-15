using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    private WorldData _worldData;
    public WorldData WorldData => _worldData;

    private GameData _gameData;
    public GameData GameData => _gameData;
    
    public UiData UiData { get; set; }

    private string WorldsFolder =>
        Path.Combine(Application.persistentDataPath, "Worlds");

    private string GamesFolder =>
        Path.Combine(Application.persistentDataPath, "Games");

    [SerializeField] private bool _useDebugGameData;
    [SerializeField] private string _debugDataName;

    private void Start()
    {
        #if UNITY_EDITOR
            if (_useDebugGameData && _gameData == null)
            {
                Debug.Log("Loading debug game data");
                LoadGameDataFromName(_debugDataName);
            }
        #endif
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

    public void SetWorldData(WorldData worldData)
    {
        _worldData = worldData;
    }

    public void SaveWorldData()
    {
        WorldSaveData saveData = _worldData.ToSaveData();
        string json = JsonUtility.ToJson(saveData, true);
        string path = WorldsFolder + "/" + saveData.WorldName + ".json";
        File.WriteAllText(path, json);
    }

    public void LoadWorldDataFromName(string worldName)
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

        SetWorldData(loadedWorld);

        MapTexture miniMapTexture =
            TextureUtilities.GetMapTextureFromWorldData(GameController.Instance.SessionManager.WorldData, 1200, 695);

        GameController.Instance.SessionManager.UiData = new UiData(miniMapTexture);
    }

    private string GetWorldPathFromName(string worldName)
    {
        return Path.Combine(WorldsFolder, worldName + ".json");
    }
    
    public void NewGameData()
    {
        _gameData = new GameData();
    }

    public void SaveGameData()
    {
        GameSaveData saveData = _gameData.ToSaveData();
        string json = JsonUtility.ToJson(saveData, true);
        string path = GamesFolder + "/" + saveData.GameName + ".json";
        File.WriteAllText(path, json);
    }

    public void LoadGameDataFromName(string gameName)
    {
        string json = File.ReadAllText(GetGamePathFromName(gameName));
        GameSaveData saveData =  JsonUtility.FromJson<GameSaveData>(json);
        
        string worldName =  saveData.WorldName;
        LoadWorldDataFromName(worldName);

        NewGameData();
        _gameData.GameName = saveData.GameName;
        _gameData.WorldName = saveData.WorldName;
        _gameData.Ticker = new Ticker(saveData.TickInfo);
        _gameData.Pops.LoadPops(saveData.Pops);
        
        foreach(Religion religion in saveData.Religions)
        {
            _gameData.Religions.Add(religion.ID, religion);
        }
        
        foreach(Culture culture in saveData.Cultures)
        {
            _gameData.Cultures.Add(culture.ID, culture);
        }
    }

    private string GetGamePathFromName(string gameName)
    {
        return Path.Combine(GamesFolder, gameName + ".json");
    }
}
