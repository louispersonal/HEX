using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public WorldData WorldData { get; private set; }
    public GameData GameData { get; private set; }
    
    private bool _sessionSet = false;
    
    public bool SessionSet => _sessionSet;
    
    public SaveManager SaveManager { get; private set; }
    
    public UiData UiData { get; set; }

    [SerializeField] private bool _useDebugGameData;
    [SerializeField] private string _debugDataName;
    
    public void SetSession(WorldData world)
    {
        SetSession(world, null);
        _sessionSet = true;
    }

    public void SetSession(WorldData world, GameData game)
    {
        WorldData = world;
        GameData = game;
        _sessionSet = true;
    }

    public void StartNewGame()
    {
        GameData = new GameData();
    }
    
    private void Start()
    {
        SaveManager = new SaveManager(this);
        #if UNITY_EDITOR
            if (_useDebugGameData && GameData == null)
            {
                // ask save manager to load debug data
            }
        #endif
    }


}
