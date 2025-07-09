using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public Hex hexTilePrefab;

    [SerializeField] int _height;
    [SerializeField] int _width;

    private float _size;

    public float Height => _size * 2;
    public float Width => _size * Mathf.Sqrt(3);

    private Dictionary<AxialCoordinate, Hex> _hexGrid;

    void Start()
    {
        _size = hexTilePrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int r = 0; r < _height; r++)
        {
            int rOffset = Mathf.FloorToInt(r / 2f); // offset for pointy-topped hexes

            for (int q = -rOffset; q < _width - rOffset; q++)
            {
                var hex = Instantiate(hexTilePrefab, HexToWorldPosition(q, r), Quaternion.identity, this.transform);
                hex.Init(q, r);
                _hexGrid.Add(new AxialCoordinate(q, r), hex);
            }
        }
    }

    Vector3 HexToWorldPosition(int q, int r)
    {
        float x = q * Width + r * Width * 0.5f;
        float y = -1 * r * 0.75f * Height;
        float z = 0f;
        return new Vector3(x, y, z);
    }

    public Hex GetHex(int q, int r)
    {
        return _hexGrid.TryGetValue(new AxialCoordinate(q, r), out var tile) ? tile : null;
    }
}
