using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexData
{
    public int Q;

    public int R;

    public TerrainType Type;

    public HexData(int q, int r, TerrainType type)
    {
        Q = q;
        R = r;
        Type = type;
    }
}

public enum TerrainType
{
    Grass,
    Sea,
    Mountain,
    Forest,
    Desert
}