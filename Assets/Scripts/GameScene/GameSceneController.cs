using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    public static GameSceneController Instance { get; private set; }

    public HexGridView HexGridView;

    public GameCamera GameCamera;
    
    public SelectionManager SelectionManager;
    
    public UiView UiView;
    
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }
}
