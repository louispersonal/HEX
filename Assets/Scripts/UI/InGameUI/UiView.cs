using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiView : MonoBehaviour
{
    [SerializeField] MiniMap _miniMap;
    
    [SerializeField] HexFlyOut _hexFlyOut;
    
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

    public void OpenHexFlyOut(HexData selectedHex)
    {
        _hexFlyOut.gameObject.SetActive(true);
        _hexFlyOut.RefreshText(selectedHex);
    }
    
    public void CloseHexFlyOut()
    {
        _hexFlyOut.gameObject.SetActive(false);
    }
}
