using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureUtilities
{
    public static Texture2D GetTexture(Color[] pixelArray, int horizontalPixels, int verticalPixels)
    {
        Texture2D tex = new Texture2D(horizontalPixels, verticalPixels);
        tex.SetPixels(pixelArray);
        tex.Apply();

        return tex;
    }

    public static Color[] GetHeightmapPixelArray(float[] heightmapArray)
    {
        Color[] pix = new Color[heightmapArray.Length];
        for (int i = 0; i < heightmapArray.Length; i++)
        {
            float colorValue = 0f;
            if (heightmapArray[i] > 0.5f) colorValue = 1f;
            pix[i] = new Color(colorValue, colorValue, colorValue);
        }
        return pix;
    }
}
