using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTexture
{
    public Texture2D Texture { get ; private set; }
    public float HexSizePixels { get ; private set; }
    
    public MapTexture(Texture2D texture, float hexSize)
    {
        Texture = texture;
        HexSizePixels = hexSize;
    }
}
