using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiData
{
    public Texture2D MiniMapTexture { get; }

    public UiData(Texture2D miniMapTexture)
    {
        MiniMapTexture = miniMapTexture;
    }
}
