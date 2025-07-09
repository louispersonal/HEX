using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Hex : MonoBehaviour
{
    private HexData _data;

    public HexData Data => _data;

    public int Q => _data.Q;

    public int R => _data.R;

    public int S => -Q - R;

    [SerializeField] TextMeshPro Q_Text;

    [SerializeField] TextMeshPro R_Text;

    [SerializeField] TextMeshPro S_Text;

    private SpriteRenderer _renderer;

    private PolygonCollider2D _polygonCollider;

    public void Init(int q, int r)
    {
        _data = new HexData(q, r, TerrainType.Sea);
        name = $"Hex {Q}, {R}";
        Q_Text.text = Q.ToString();
        R_Text.text = R.ToString();
        S_Text.text = S.ToString();
        _renderer = GetComponent<SpriteRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();

        SwitchSprite();
    }

    public void SwitchSprite()
    {
        switch (_data.Type)
        {
            case TerrainType.Grass:
                _renderer.color = Color.green;
                break;
            case TerrainType.Sea:
                _renderer.color = Color.blue;
                break;
            case TerrainType.Mountain:
                _renderer.color = Color.gray;
                break;
            case TerrainType.Forest:
                _renderer.color = new Color(0, 0.5f, 0, 1);
                break;
            case TerrainType.Desert:
                _renderer.color = Color.yellow;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_polygonCollider.OverlapPoint(mouseWorldPos))
            {
                Debug.Log("Clicked on polygon collider!");
                ToggleType();
            }
        }
    }

    private void ToggleType()
    {
        _data.Type = (TerrainType)(((int)_data.Type + 1) % 5);

        SwitchSprite();
    }
}
