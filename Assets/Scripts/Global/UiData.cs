using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiData
{
    public MapTexture MiniMapTexture { get; }

    public UiData(MapTexture miniMapTexture)
    {
        MiniMapTexture = miniMapTexture;
    }
}
