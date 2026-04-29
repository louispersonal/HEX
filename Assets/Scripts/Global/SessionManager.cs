using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    private WorldData _worldData;
    public WorldData WorldData { get { return _worldData; } }

    private GameData _gameData;

    public void LoadWorldData(string filename)
    {
        // load in hex list
        List<HexData> data = new List<HexData>();
        _worldData = new WorldData(data);
    }

    public void SetWorldData(WorldData worldData)
    {
        _worldData = worldData;
    }

    public void SaveWorldData()
    {
        WorldSaveData saveData = _worldData.ToSaveData();
        string json = JsonUtility.ToJson(saveData, true);
        string path = Application.persistentDataPath + "/world_123.json";
        System.IO.File.WriteAllText(path, json);
    }

    public void LoadGameData(string filename)
    {

    }

    public void NewGameData()
    {

    }

    public void SaveGameData()
    {

    }
}
