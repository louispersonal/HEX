using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BaseHexGrid : MonoBehaviour
{
    public static BaseHexGrid Instance { get; private set; }

    private Dictionary<AxialCoordinate, BaseHex> _hexGrid;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _hexGrid = new Dictionary<AxialCoordinate, BaseHex>();
    }

    public bool TryGetHex(AxialCoordinate coord, out BaseHex hex)
    => _hexGrid.TryGetValue(coord, out hex);

    public void GenerateGrid(int N)
    {
        int hexCount = 1 + 3 * N * (N + 1);

        _hexGrid = new Dictionary<AxialCoordinate, BaseHex>(hexCount);

        for (int q = -N; q <= N; q++)
        {
            for (int r = Mathf.Max(-N, -q - N); r <= Mathf.Min(N, -q + N); r++)
            {
                BaseHex currentHex = new BaseHex(q, r);
                _hexGrid.Add(currentHex.Coord, currentHex);
            }
        }
    }

    public float DistanceBetweenHexes(BaseHex a, BaseHex b)
    {
        return AxialCoordinate.DistanceBetweenCoords(a.Coord, b.Coord);
    }

    public List<BaseHex> HexesWithinRadiusOfHex(BaseHex a, int radius)
    {
        List<BaseHex> hexesInRange = new List<BaseHex>();

        for (int q = -radius; q <= radius; q++)
        {
            for (int r = Mathf.Max(-radius, -q - radius); r <= Mathf.Min(radius, -q + radius); r++)
            {
                AxialCoordinate hexCoord = a.Coord + new AxialCoordinate(q, r);
                if (Instance.TryGetHex(hexCoord, out BaseHex neighborHex))
                {
                    hexesInRange.Add(neighborHex);
                }
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

    public BaseHex GetHexAtScenePoint(Vector2 p)
    {
        if (Instance.TryGetHex(SceneToAxialConversion(p), out BaseHex hex))
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
            hexes = new List<HexEntry>(_hexGrid.Count)
        };

        foreach (var kv in _hexGrid)
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

        _hexGrid = new Dictionary<AxialCoordinate, BaseHex>(save.hexes.Count);
        foreach (var e in save.hexes)
        {
            var coord = new AxialCoordinate(e.Hex.Coord.Q, e.Hex.Coord.R);
            var hex =e.Hex;
            _hexGrid[coord] = hex;
        }

        Debug.Log($"Loaded hex grid ({save.hexes.Count} tiles) from {path}");

        return true;
    }
}

[System.Serializable]
public class HexEntry
{
    public BaseHex Hex;
}

[System.Serializable]
public class HexGridSave
{
    public int version = 1;
    public List<HexEntry> hexes;
}