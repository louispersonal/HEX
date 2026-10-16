using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public WorldData WorldData { get; private set; }
    public GameData GameData { get; private set; }
    
    private bool _worldDataSet = false;
    
    private bool _gameDataSet = false;
    
    public bool SessionSet => _worldDataSet && _gameDataSet;
    
    public SaveManager SaveManager { get; private set; }
    
    public UiData UiData { get; set; }

    [SerializeField] private bool _useDebugGameData;
    [SerializeField] private string _debugDataName;
    
    public void SetSession(WorldData world)
    {
        SetSession(world, null);
        _worldDataSet = true;
    }

    public void SetSession(WorldData world, GameData game)
    {
        WorldData = world;
        GameData = game;
        RegisterTickables();
        _gameDataSet = true;
    }

    public void StartNewGame()
    {
        GameData = new GameData();
    }

    private void RegisterTickables()
    {
        foreach (ITickable tickable in GameData.GetTickables())
        {
            GameData.Ticker.Register(tickable);
        }
    }
    
    private void Awake()
    {
        SaveManager = new SaveManager(this);
    }

    private void Start()
    {
        #if UNITY_EDITOR
            if (_useDebugGameData && !SessionSet)
            {
                // ask save manager to load debug data
            }
        #endif
    }


}
