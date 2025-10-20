using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    public MapDefinition MapDefinition { get; private set; }

    public BaseHexGrid HexGrid { get; private set; }

    private void Awake()
    {
        // Singleton Block
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Instantiate data
        MapDefinition = new MapDefinition();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadHexGrid()
    {
        HexGrid = new BaseHexGrid();
    }

    public void NewHexGrid()
    {
        HexGrid = new BaseHexGrid();
        HexGrid.GenerateRectangularGrid(MapDefinition.FBMParams.WorldHeight, MapDefinition.FBMParams.WorldWidth);
    }
}

[System.Serializable]
public class MapDefinition
{
    public string Name;
    public FBMParams FBMParams;

    public MapDefinition()
    {
        Name = "No name";
        FBMParams = new FBMParams();
    }
}


[System.Serializable]
public class FBMParams
{
    public int Octaves;
    public float Lacunarity;
    public float Gain;
    public float Seed;
    public float SeaLevel;
    public int WorldWidth;
    public int WorldHeight;
    public float Archipelagoness;
}