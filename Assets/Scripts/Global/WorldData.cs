using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldData
{
    private string _name = "TestWorld";
    public string Name { get { return _name; } set { _name = value; } }

    private HexGrid _grid;

    public HexGrid Grid { get { return _grid; } }

    private Pathfinder _pathFinder;

    public Pathfinder Pathfinder { get { return _pathFinder; } }

    public SpatialLookup<RiverID, River> Rivers = new();
    public SpatialLookup<LakeID, Lake> Lakes = new();
    public SpatialLookup<GeoID, GeoFeature> GeoFeatures = new();

    public IndexedDictionary<RegionID, Region> Regions = new();  

    public WorldData(List<Hex> hexDataList)
    {
        _grid = new HexGrid(hexDataList);
        _pathFinder = new Pathfinder(_grid);
    }
    
    public WorldSaveData ToSaveData()
    {
        return new WorldSaveData(_name, Grid.GetHexData(), Rivers.ToArray(), Lakes.ToArray(), GeoFeatures.ToArray(), Regions.ToArray());
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