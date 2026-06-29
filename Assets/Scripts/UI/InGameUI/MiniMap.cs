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
    
    public void SetTexture(Texture2D texture)
    {
        _miniMapImage.texture = texture;
        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized) return;
    }
}
