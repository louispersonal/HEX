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

    public Pathfinder PathFinder { get { return _pathFinder; } }

    public SpatialLookup<RiverID, River> Rivers = new();
    public SpatialLookup<LakeID, Lake> Lakes = new();
    public SpatialLookup<GeoID, GeoFeature> GeoFeatures = new();

    public Region[] Regions = new Region[0];

    public WorldData(List<HexData> hexDataList)
    {
        _grid = new HexGrid(hexDataList);
        _pathFinder = new Pathfinder(_grid);
    }

    public WorldSaveData ToSaveData()
    {
        return new WorldSaveData(_name, Grid.GetHexData(), Rivers.Objects.Values.ToList(), Lakes.Objects.Values.ToList(), GeoFeatures.Objects.Values.ToList(), Regions);
    }
}

[System.Serializable]
public class WorldSaveData
{
    public string WorldName;
    public List<HexData> Hexes;
    public List<River> Rivers;
    public List<Lake> Lakes;
    public List<GeoFeature> GeoFeatures;
    public Region[] Regions;

    public WorldSaveData(string name, List<HexData> hexes, List<River> rivers, List<Lake> lakes, List<GeoFeature> geoFeatures, Region[] regions)
    {
        WorldName = name;
        Hexes = hexes;
        Rivers = rivers;
        Lakes = lakes;
        GeoFeatures = geoFeatures;
        Regions = regions;
    }
}