using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiView : MonoBehaviour
{
    [SerializeField] MiniMap _miniMap;
    
    [SerializeField] HRFlyout _hrpflyout;
    
    public MapTexture MiniMap { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        if (MiniMap == null) MiniMap = GameController.Instance.SessionManager.WorldData.GenerateMiniMap();
        
        _miniMap.SetTexture(MiniMap);
    }
}
