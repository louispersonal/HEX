using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class SessionManager : MonoBehaviour
{
    private WorldData _worldData;
    public WorldData WorldData { get { return _worldData; } }

    private GameData _gameData;

    private string WorldsFolder =>
    Path.Combine(Application.persistentDataPath, "Worlds");

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

    public void LoadWorldData(string worldName)
    {
        string filename = Path.Combine(WorldsFolder, worldName + ".json");
        string json = File.ReadAllText(filename);
        WorldSaveData saveData = JsonUtility.FromJson<WorldSaveData>(json);
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
