using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BaseHexGrid : MonoBehaviour
{
    public static BaseHexGrid Instance { get; private set; }

    public Dictionary<AxialCoordinate, Hex> Grid { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Grid = new Dictionary<AxialCoordinate, Hex>();
    }

    public bool TryGetHex(AxialCoordinate coord, out Hex hex)
    => Grid.TryGetValue(coord, out hex);

    public void GenerateHexShapedGrid(int N)
    {
        int hexCount = 1 + 3 * N * (N + 1);

        Grid = new Dictionary<AxialCoordinate, Hex>(hexCount);

        for (int q = -N; q <= N; q++)
        {
            for (int r = Mathf.Max(-N, -q - N); r <= Mathf.Min(N, -q + N); r++)
            {
                Hex currentHex = new Hex(q, r);
                Grid.Add(currentHex.Coord, currentHex);
            }
        }
    }

    public void GenerateRectangularGrid(int columns, int rows)
    {
        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                Hex currentHex = new Hex(AxialCoordinate.OddRToAxial((r, c)));
                Grid.Add(currentHex.Coord, currentHex);
            }
        }
    }

    public float DistanceBetweenHexes(Hex a, Hex b)
    {
        return AxialCoordinate.DistanceBetweenCoords(a.Coord, b.Coord);
    }

    public List<Hex> HexesWithinRadiusOfHex(Hex a, int radius)
    {
        List<Hex> hexesInRange = new List<Hex>();

        List<AxialCoordinate> axials = AxialCoordinate.CoordsWithinRadiusOfCoord(a.Coord, radius);

        foreach (AxialCoordinate axial in axials)
        {
            if (Instance.TryGetHex(axial, out Hex neighborHex))
            {
                hexesInRange.Add(neighborHex);
            }
        }

        return hexesInRange;
    }

    public List<Hex> HexesInRingOfRadiusOfHex(Hex a, int radius)
    {
        List<Hex> hexesInRange = new List<Hex>();

        List<AxialCoordinate> axials = AxialCoordinate.CoordsInRingOfRadius(a.Coord, radius);

        foreach (AxialCoordinate axial in axials)
        {
            if (Instance.TryGetHex(axial, out Hex neighborHex))
            {
                hexesInRange.Add(neighborHex);
            }
        }

        return hexesInRange;
    }

    public Vector2 AxialToSceneConversion(AxialCoordinate a)
    {
        float x = BaseHex.SceneSize * Mathf.Sqrt(3f) * (a.Q + (a.R / 2f));
        float y = BaseHex.SceneSize * (1.5f) * a.R;
        return new Vector2(x, y);
    }

    public AxialCoordinate SceneToAxialConversion(Vector2 p)
    {
        float r = p.y * (2f / (3f * BaseHex.SceneSize));
        float q = (p.x / (Mathf.Sqrt(3f) * BaseHex.SceneSize)) - (p.y / (3f * BaseHex.SceneSize));

        return new AxialCoordinate((int)Mathf.Round(q), (int)Mathf.Round(r));
    }

    public Hex GetHexAtScenePoint(Vector2 p)
    {
        if (Instance.TryGetHex(SceneToAxialConversion(p), out Hex hex))
        {
            return hex;
        }
        Debug.Log("No hex at point: " + p.ToString());
        return null;
    }

    public void SaveToFile(string fileName)
    {
        var save = new HexGridSave
        {
            version = 1,
            hexes = new List<HexEntry>(Grid.Count)
        };

        foreach (var kv in Grid)
        {
            save.hexes.Add(new HexEntry
            {
                Hex = kv.Value
            });
        }

        var json = JsonUtility.ToJson(save, prettyPrint: true);
        var path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);

        Debug.Log($"Saved hex grid ({save.hexes.Count} tiles) to {path}");
    }

    public bool LoadFromFile(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Save file not found: {path}");

            return false;
        }

        var json = File.ReadAllText(path);
        var save = JsonUtility.FromJson<HexGridSave>(json);
        if (save == null || save.hexes == null)
        {
            Debug.LogError("Failed to parse save file.");

            return false;
        }

        Grid = new Dictionary<AxialCoordinate, Hex>(save.hexes.Count);
        foreach (var e in save.hexes)
        {
            var coord = new AxialCoordinate(e.Hex.Coord.Q, e.Hex.Coord.R);
            var hex =e.Hex;
            Grid[coord] = hex;
        }

        Debug.Log($"Loaded hex grid ({save.hexes.Count} tiles) from {path}");

        return true;
    }
}

[System.Serializable]
public class HexEntry
{
    public Hex Hex;
}

[System.Serializable]
public class HexGridSave
{
    public int version = 1;
    public List<HexEntry> hexes;
}