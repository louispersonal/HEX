using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.UI.Image;

public class WorldSeedingMenuController : MonoBehaviour
{
    [SerializeField] MenuView _worldSeedingMenuView;
    [SerializeField] UnityEngine.UI.RawImage _previewImage;

    [SerializeField] List<Color> _biomeColors;

    const float SQRT3 = 1.7320508075688772f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SeedWorld()
    {
        WorldSeedingController.SeedWorld(WorldManager.Instance.HexGrid, WorldManager.Instance.MapDefinition);
        PreviewMap();
    }

    public static (float q, float r) PixelToFractionalAxialConversion(Vector3 p, float hexPixelSize)
    {
        float r = p.y * (2f / (3f * hexPixelSize));
        float q = (p.x / (Mathf.Sqrt(3f) * hexPixelSize)) - (p.y / (3f * hexPixelSize));

        return (q, r);
    }

    public void EnableMenu()
    {
        _worldSeedingMenuView.gameObject.SetActive(true);
    }

    public void DisableMenu()
    {
        _worldSeedingMenuView.gameObject.SetActive(false);
    }

    public void PreviewMap()
    {
        Texture2D tex = BuildPreviewTexture(1600, 800, 0);
        _previewImage.texture = tex;
    }

    public void Play()
    {
        GameController.Instance.GoToScene(SceneNames.Game);
    }

    Texture2D BuildPreviewTexture(int texW, int texH, int margin)
    {
        // 0) Collect Odd-R cells (or convert from axial once)
        var cells = new List<(int col, int row)>();
        foreach (Hex hex in WorldManager.Instance.HexGrid.Grid.Values)
        {
            var (row, col) = hex.Coord.ConvertToOddR();
            cells.Add((col, row));
        }

        // 1) Fit
        ComputeSizeAndOrigin(cells, texW, texH, margin, out float size, out Vector2 origin);

        // 2) Prepare texture buffer
        var tex = new Texture2D(texW, texH, TextureFormat.RGBA32, false);
        var pix = new Color[texW * texH];
        // optional background: fill with Color.clear or a sky color

        // 3) For each hex, compute center and paint the filled hex
        foreach (Hex hex in WorldManager.Instance.HexGrid.Grid.Values)
        {
            var (row, col) = hex.Coord.ConvertToOddR();
            Vector2 center = OddRToPixel((col, row), size, origin);

            Color c = GetColorForBiome(hex.Biome);
            DrawFilledHex(pix, texW, texH, center, size, c);
        }

        // 4) Upload once
        tex.SetPixels(pix);
        tex.Apply(false, false);
        return tex;
    }

    // Fit & center from your earlier step (Odd-R, pointy-top)
    public static void ComputeSizeAndOrigin(IReadOnlyCollection<(int col, int row)> oddRCells, int pixelWidth, int pixelHeight, float margin, out float size, out Vector2 origin)
    {
        if (oddRCells == null || oddRCells.Count == 0)
            throw new ArgumentException("No cells.");

        int minRow = int.MaxValue, maxRow = int.MinValue;
        float minTerm = float.PositiveInfinity, maxTerm = float.NegativeInfinity;

        foreach (var (col, row) in oddRCells)
        {
            int parity = row & 1;
            float leftTerm = col + 0.5f * parity - 0.5f;
            float rightTerm = col + 0.5f * parity + 0.5f;

            if (leftTerm < minTerm) minTerm = leftTerm;
            if (rightTerm > maxTerm) maxTerm = rightTerm;

            if (row < minRow) minRow = row;
            if (row > maxRow) maxRow = row;
        }

        float spanTerm = Mathf.Max(0f, maxTerm - minTerm);
        float spanRows = Mathf.Max(0, maxRow - minRow);

        float usableW = (float)pixelWidth - 2f * margin;
        float usableH = (float)pixelHeight - 2f * margin;

        float sizeFromW = (spanTerm > 0f) ? (usableW / (SQRT3 * spanTerm)) : float.PositiveInfinity;
        float sizeFromH = (usableH / (1.5f * spanRows + 2f));
        size = Mathf.Max(0.0001f, Mathf.Min(sizeFromW, sizeFromH));

        float w = SQRT3 * size;

        float originX = margin - w * minTerm;
        float originY = margin - (1.5f * size * minRow - size);

        float gridW = w * spanTerm;
        float gridH = size * (1.5f * spanRows + 2f);
        float extraW = usableW - gridW;
        float extraH = usableH - gridH;

        originX += 0.5f * Mathf.Max(0f, extraW);
        originY += 0.5f * Mathf.Max(0f, extraH);

        origin = new Vector2(originX, originY);
    }

    // Odd-R (pointy-top) -> pixel center
    public static Vector2 OddRToPixel((int col, int row) h, float size, Vector2 origin)
    {
        float w = SQRT3 * size;
        float x = origin.x + w * (h.col + 0.5f * (h.row & 1));
        float y = origin.y + 1.5f * size * h.row;
        return new Vector2(x, y);
    }

    // Draw a filled hex of radius `size` at pixel center `c`, into `pix` buffer
    public static void DrawFilledHex(
        Color[] pix, int texW, int texH,
        Vector2 c, float size, Color color)
    {
        float apothem = 0.5f * SQRT3 * size;  // horizontal half-extent
        int minX = Mathf.Max(0, Mathf.FloorToInt(c.x - apothem));
        int maxX = Mathf.Min(texW - 1, Mathf.CeilToInt(c.x + apothem));
        int minY = Mathf.Max(0, Mathf.FloorToInt(c.y - size));
        int maxY = Mathf.Min(texH - 1, Mathf.CeilToInt(c.y + size));

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
                if (SQRT3 * ax + ay > 2f * size) continue; // outside slants

                pix[y * texW + x] = color;
            }
        }
    }

    public Color GetColorForBiome(Biome biome)
    {
        if (_biomeColors.Count < 7)
        {
            Debug.LogError("You need to populate the biome colors");
        }

        return _biomeColors[(int)biome];
    }
}
