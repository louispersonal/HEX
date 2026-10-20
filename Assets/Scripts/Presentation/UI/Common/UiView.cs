using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiView : MonoBehaviour
{
    [SerializeField] MiniMap _miniMap;
    
    [SerializeField] HRFlyout _hrpflyout;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameController.Instance.SessionManager.WorldData.MiniMap == null)
            GameController.Instance.SessionManager.WorldData.GenerateMiniMap();
        
        _miniMap.SetTexture(GameController.Instance.SessionManager.WorldData.MiniMap);
    }
}
