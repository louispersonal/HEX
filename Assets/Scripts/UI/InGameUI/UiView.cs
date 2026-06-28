using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiView : MonoBehaviour
{
    [SerializeField] MiniMap _miniMap;
    // Start is called before the first frame update
    void Start()
    {
        if (GameController.Instance.SessionManager.UiData.MiniMapTexture != null)
        {
            _miniMap.SetTexture(GameController.Instance.SessionManager.UiData.MiniMapTexture);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
