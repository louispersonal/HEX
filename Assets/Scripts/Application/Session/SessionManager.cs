using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public WorldData WorldData { get; private set; }
    public GameData GameData { get; private set; }
    
    public bool WorldDataSet => WorldData != null;
    public bool GameDataSet => GameData != null;
    public bool SessionSet => WorldDataSet && GameDataSet;
    
    public SaveManager SaveManager { get; private set; }

    [SerializeField] private bool _useDebugGameData;
    [SerializeField] private string _debugDataName;

    public void SetSession(WorldData world, GameData game = null)
    {
        WorldData = world;
        GameData = game;
        
        if (game?.Ticker != null)
        {
            RegisterTickables();
        }
    }

    public void StartNewGame()
    {
        GameData = new GameData();
        TickInfo newTickInfo =  new TickInfo();
        Ticker newTicker = new Ticker(newTickInfo);
        GameController.Instance.SessionManager.GameData.Ticker = newTicker;
        GameController.Instance.SessionManager.GameData.GameName = "Debug";
        GameController.Instance.SessionManager.GameData.WorldName = GameController.Instance.SessionManager.WorldData.Name;
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
