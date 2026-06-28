using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private RawImage _miniMapImage;

    public void SetTexture(Texture2D texture)
    {
        _miniMapImage.texture = texture;
    }
}
