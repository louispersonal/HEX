using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class FullView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;

    private MapTexture _mapTexture;
    
    public WorldData WorldData => GameController.Instance.SessionManager.WorldData;
    
    private HexGridView _hexGridView;

    public void Initialize(HexGridView hexGridView)
    {
        _hexGridView = hexGridView;
        GenerateMapTexture();
        float horizontalSpan = _hexGridView.WorldCorners[3].x - hexGridView.WorldCorners[0].x;
        float verticalSpan = _hexGridView.WorldCorners[1].y - hexGridView.WorldCorners[0].y;
        Vector2 centerPoint = new Vector2(hexGridView.WorldCorners[0].x + horizontalSpan / 2,
            hexGridView.WorldCorners[0].y + verticalSpan / 2);
        
        _renderer.gameObject.transform.position = centerPoint;
        _renderer.gameObject.transform.localScale = new Vector3(horizontalSpan, verticalSpan, 1f);

        _renderer.material.mainTexture = _mapTexture.Texture;
        
        _renderer.enabled = false;
    }

    private void GenerateMapTexture()
    {
        _mapTexture = TextureUtilities.GetMapTextureFromWorldData(WorldData, 
            GameConfigurations.MaxResolution.w, 
            GameConfigurations.MaxResolution.h);
    }
}
