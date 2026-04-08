using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData
{
    private HexGrid _grid;

    public HexGrid Grid { get { return _grid; } }

    private Pathfinder _pathFinder;

    public Pathfinder PathFinder { get { return _pathFinder; } }

    public Dictionary<RiverID, River> Rivers;
    public Dictionary<AxialCoordinate, RiverID> RiverLookup;

    public WorldData(List<HexData> hexDataList)
    {
        _grid = new HexGrid(hexDataList);
        _pathFinder = new Pathfinder(_grid);

        Rivers = new Dictionary<RiverID, River>();
        RiverLookup = new Dictionary<AxialCoordinate, RiverID>();
    }

    public List<HexData> GetHexData()
    {
        return Grid.GetHexData();
    }
} 
