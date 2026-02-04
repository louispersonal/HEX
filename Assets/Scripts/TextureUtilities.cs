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

    // Draw a filled hex of radius `size` at pixel center `c`, into `pix` buffer
    public static void DrawFilledHex(Color[] pix, int horizontalPixels, int verticalPixels, Vector2 c, float size, Color color)
    {
        float apothem = 0.5f * AxialGeometry.SQRT3 * size;  // horizontal half-extent
        int minX = Mathf.Max(0, Mathf.FloorToInt(c.x - apothem));
        int maxX = Mathf.Min(horizontalPixels - 1, Mathf.CeilToInt(c.x + apothem));
        int minY = Mathf.Max(0, Mathf.FloorToInt(c.y - size));
        int maxY = Mathf.Min(verticalPixels - 1, Mathf.CeilToInt(c.y + size));

        // scan the tight AABB
        for (int y = minY; y <= maxY; y++)
        {
            float py = (y + 0.5f) - c.y;  // sample pixel center
            float ay = Mathf.Abs(py);
            if (ay > size) continue; // outside top/bottom

            for (int x = minX; x <= maxX; x++)
            {
                float px = (x + 0.5f) - c.x;
                float ax = Mathf.Abs(px);

                // fast hex-inside test (pointy-top)
                if (ax > apothem) continue;                // outside verticals
                if (AxialGeometry.SQRT3 * ax + ay > 2f * size) continue; // outside slants

                pix[y * horizontalPixels + x] = color;
            }
        }
    }
}
