using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData
{
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
        return new WorldSaveData(Grid.GetHexData());
    }
}

[System.Serializable]
public class WorldSaveData
{
    public List<HexData> Hexes;

    public WorldSaveData(List<HexData> hexes)
    {
        Hexes = hexes;
    }
}