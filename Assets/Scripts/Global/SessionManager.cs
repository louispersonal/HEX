using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    private WorldData _worldData;
    public WorldData WorldData { get { return _worldData; } }

    private GameData _gameData;

    private string WorldsFolder =>
    Path.Combine(Application.persistentDataPath, "Worlds");

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
        string path = WorldsFolder + "/world_" + saveData.WorldName + ".json";
        File.WriteAllText(path, json);
    }

    public void LoadGameData(string filename)
    {
        WorldSaveData saveData = JsonUtility.FromJson<WorldSaveData>(filename);
        WorldData loadedWorld = new WorldData(saveData.Hexes);
        
        foreach (River river in saveData.Rivers)
        {
            loadedWorld.Rivers.Add(river.ID, river, river.Coords);
        }

        foreach (Lake lake in saveData.Lakes)
        {
            loadedWorld.Lakes.Add(lake.ID, lake, lake.Coords);
        }

        foreach (GeoFeature geoFeature in saveData.GeoFeatures)
        {
            loadedWorld.GeoFeatures.Add(geoFeature.ID, geoFeature, geoFeature.Coords);
        }

        loadedWorld.Regions = saveData.Regions;

        SetWorldData(loadedWorld);
    }

    public void NewGameData()
    {

    }

    public void SaveGameData()
    {

    }
}
