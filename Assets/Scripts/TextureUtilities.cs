using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

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

    public static void DrawFilledHex(Color[] pix, int horizontalPixels, Vector2 c, int outerRadius, Color color)
    {
        int innerRadius = Mathf.RoundToInt((AxialGeometry.SQRT3 * 0.5f) * outerRadius);

        int minY = (int)c.y - outerRadius;
        int maxY = (int)c.y + outerRadius;

        for (int y = minY; y <= maxY; y++)
        {
            int distanceToCenter = Mathf.Abs(y - (int)c.y);
            if (distanceToCenter < outerRadius / 2)
            {
                int minX = (int)c.x - innerRadius;
                int maxX = (int)c.x + innerRadius;
                for (int x = minX; x < maxX; x++)
                {
                    pix[y * horizontalPixels + x] = color;
                }
            }
            else
            {
                float orientation = y > c.y? 1 : -1;
                Vector2 P_0 = new Vector2(c.x, c.y + (orientation * outerRadius) / 2);
                Vector2 P_1 = new Vector2(P_0.x + innerRadius, P_0.y);
                Vector2 P_2 = new Vector2(P_0.x, P_0.y + (orientation * outerRadius) / 2);
                Vector2 P_3 = new Vector2(P_0.x - innerRadius, P_0.y);
                int minX = Mathf.RoundToInt(P_3.x + ((P_0.x - P_3.x) * ((y - P_0.y) / (P_2.y - P_0.y))));
                int maxX = Mathf.RoundToInt(P_1.x + ((P_0.x - P_1.x) * ((y - P_0.y) / (P_2.y - P_0.y))));
                for (int x = minX; x < maxX; x++)
                {
                    pix[y * horizontalPixels + x] = color;
                }
            }
        }
    }

    public static void DrawLine(Color[] pix, int horizontalPixels, Vector2 lineStart, Vector2 lineEnd, Color color, int width=1)
    {
        List<Vector2Int> points = CartesianGeometry.BresenhamsLine(new Vector2Int (Mathf.RoundToInt(lineStart.x), Mathf.RoundToInt(lineStart.y)), new Vector2Int(Mathf.RoundToInt(lineEnd.x), Mathf.RoundToInt(lineEnd.y)), width);

        foreach (Vector2Int point in points)
        {
            pix[point.y * horizontalPixels + point.x] = color;
        }
    }

    public static void DrawDot(Color[] pix, int horizontalPixels, Vector2 position, int radius, Color color)
    {
        Vector2Int snappedPosition = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));

        List<Vector2Int> points = CartesianGeometry.GetCircle(snappedPosition, radius);

        foreach (Vector2Int point in points)
        {
            pix[point.y * horizontalPixels + point.x] = color;
        }
    }

    public static Color[] GetPixelsFromWorldData(WorldData world, int horizontalPixels, int verticalPixels)
    {
        HexGrid grid = world.Grid;

        Color[] pixelArray = new Color[horizontalPixels * verticalPixels];

        for (int p = 0; p < horizontalPixels * verticalPixels; p++)
        {
            pixelArray[p] = Color.blue;
        }

        var coords = AxialGeometry.ConvertAxialSetToBoundedCartesian(grid.GetAllAxialCoords(), Vector2.zero, new Vector2(horizontalPixels, verticalPixels), out float size);

        foreach (AxialCoordinate axial in coords.Keys)
        {
            if (grid.TryGetHex(axial, out HexData data))
            {
                Vector2 pixelCoord = coords[axial];
                DrawFilledHex(pixelArray, horizontalPixels, pixelCoord, Mathf.RoundToInt(size), GetBiomeColor(data));
            }
        }

        // Add rivers
        foreach (River river in world.Rivers.Objects.Values)
        {
            for (int i = 0; i < river.Coords.Count - 1; i++)
            {
                Vector2 pixelCoordRiverStart = coords[river.Coords[i]];
                Vector2 pixelCoordRiverEnd = coords[river.Coords[i + 1]];
                DrawLine(pixelArray, horizontalPixels, pixelCoordRiverStart, pixelCoordRiverEnd, new Color(0.26f, 0.94f, 0.96f), Mathf.RoundToInt(size * 0.75f));
            }
        }

        return pixelArray;
    }

    public static Color[] GetMapModePixelsFromWorldData(WorldData world, int horizontalPixels, int verticalPixels, Color color, MapModeTypes type)
    {
        HexGrid grid = world.Grid;

        Color[] pixelArray = new Color[horizontalPixels * verticalPixels];

        for (int p = 0; p < horizontalPixels * verticalPixels; p++)
        {
            pixelArray[p] = Color.black;
        }

        var coords = AxialGeometry.ConvertAxialSetToBoundedCartesian(grid.GetAllAxialCoords(), Vector2.zero, new Vector2(horizontalPixels, verticalPixels), out float size);

        foreach (AxialCoordinate axial in coords.Keys)
        {
            if (grid.TryGetHex(axial, out HexData data))
            {
                Vector2 pixelCoord = coords[axial];

                float parameterValue = 0f;

                switch (type)
                {
                    case MapModeTypes.Elevation:
                        parameterValue = data.ExtraData.Elevation;
                        break;
                    case MapModeTypes.Temperature:
                        parameterValue = data.ExtraData.Temperature;
                        break;
                    case MapModeTypes.Precipitation:
                        parameterValue = data.ExtraData.Precipitation;
                        break;
                    case MapModeTypes.Vegetation:
                        parameterValue = data.ExtraData.LowVegetation;
                        break;
                }

                Color.RGBToHSV(color, out float h, out float s, out float v);
                v *= parameterValue;
                Color newColor = Color.HSVToRGB(h, s, v);

                DrawFilledHex(pixelArray, horizontalPixels, pixelCoord, Mathf.RoundToInt(size), newColor);
            }
        }

        return pixelArray;
    }

    public static Color GetBiomeColor(HexData data)
    {
        switch (data.ExtraData.Biome)
        {
            case Biome.Taiga:
                return new Color(0.8f, 1f, 1f);
            case Biome.Temperate:
                return new Color(0.4f, 1f, 0.5f);
            case Biome.Desert:
                return new Color(1f, 1f, 0.6f);
            case Biome.Tropical:
                return new Color(0f, 0.6f, 0f);
            case Biome.Steppe:
                return new Color(0.9f, 1f, 0.8f);
            case Biome.Savanna:
                return new Color(1f, 0.4f, 0.2f);
            case Biome.Tundra:
                return Color.white;
            case Biome.Sea:
                return Color.blue;
        }
        return Color.blue;
    }
}

public enum MapModeTypes
{
    Elevation,
    Temperature,
    Precipitation,
    Vegetation,
    General
}