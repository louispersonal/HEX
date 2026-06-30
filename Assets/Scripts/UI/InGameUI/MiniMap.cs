using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private RawImage _miniMapImage;
    [SerializeField] private Image _windowImage;

    private bool _initialized = false;
    private Vector3[] _corners; // 0 bottom left / clockwise
    
    public void SetTexture(Texture2D texture)
    {
        _miniMapImage.texture = texture;
        InitializePositions();
        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized) return;
    }

    private void InitializePositions()
    {
        _corners = new Vector3[4];
        _miniMapImage.rectTransform.GetWorldCorners(_corners);
    }

    private (float q, float r) GetCameraAxialPosition()
    {
        return GameSceneController.Instance.HexGridView.CameraCenter;
    }
}
