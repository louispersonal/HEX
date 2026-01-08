using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData
{
    private HexGrid _grid;

    public HexGrid Grid { get { return _grid; } }

    public WorldData(List<HexData> hexDataList)
    {
        _grid = new HexGrid(hexDataList);
    }

    public List<HexData> GetHexData()
    {
        return Grid.GetHexData();
    }
}
