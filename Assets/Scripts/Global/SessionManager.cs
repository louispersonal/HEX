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

    public void NewWorldData(int seed, int worldWidthInHexes)
    {
        _worldData = WorldGenController.GenerateWorldData(worldWidthInHexes, seed);
    }

    public void SaveWorldData()
    {
        List<HexData> data = _worldData.GetHexData();
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
