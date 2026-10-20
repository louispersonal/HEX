using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldData
{
    public string Name { get; }

    private HexGrid _grid;

    public HexGrid Grid => _grid;

    public Pathfinder Pathfinder { get; }

    public SpatialLookup<RiverID, River> Rivers = new();
    public SpatialLookup<LakeID, Lake> Lakes = new();
    public SpatialLookup<GeoID, GeoFeature> GeoFeatures = new();

    public IndexedDictionary<RegionID, Region> Regions = new();  

    public MapTexture MiniMap { get; private set; }
    
    public WorldData(string worldName, List<Hex> hexDataList)
    {
        Name = worldName;
        _grid = new HexGrid(hexDataList);
        Pathfinder = new Pathfinder(_grid);
    }

    public WorldData(WorldSaveData saveData)
    {
        WorldData loadedWorld = new WorldData(saveData.WorldName, saveData.Hexes);
        
        foreach (River river in saveData.Rivers)
        {
            river.PopulateRiverConnections();
        }
        
        loadedWorld.Rivers = new SpatialLookup<RiverID, River>(saveData.Rivers);
        loadedWorld.Lakes = new SpatialLookup<LakeID, Lake>(saveData.Lakes);
        loadedWorld.GeoFeatures = new SpatialLookup<GeoID, GeoFeature>(saveData.GeoFeatures);

        loadedWorld.Regions = new IndexedDictionary<RegionID, Region>(saveData.Regions);
        
        GenerateMiniMap();
    }

    public void GenerateMiniMap()
    {
        MiniMap = TextureUtilities.GetMapTextureFromWorldData(this, 
            GameConfigurations.MiniMapResolution.w, 
            GameConfigurations.MiniMapResolution.h);
    }
    
    public WorldSaveData ToSaveData()
    {
        return new WorldSaveData(Name, Grid.GetHexData(), Rivers.ToArray(), Lakes.ToArray(), GeoFeatures.ToArray(), Regions.ToArray());
    }
}

[System.Serializable]
public class WorldSaveData
{
    public string WorldName;
    public List<Hex> Hexes;
    public River[] Rivers;
    public Lake[] Lakes;
    public GeoFeature[] GeoFeatures;
    public Region[] Regions;

    public WorldSaveData(string name, List<Hex> hexes, River[] rivers, Lake[] lakes, GeoFeature[] geoFeatures, Region[] regions)
    {
        WorldName = name;
        Hexes = hexes;
        Rivers = rivers;
        Lakes = lakes;
        GeoFeatures = geoFeatures;
        Regions = regions;
    }
}