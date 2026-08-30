using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiView : MonoBehaviour
{
    [SerializeField] MiniMap _miniMap;
    
    [SerializeField] HRPFlyout _hrpflyout;
    
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

    public void OpenFlyOut(Hex hex, Region region, Pop pop)
    {
        _hrpflyout.OpenFlyOut();
        _hrpflyout.SetSelection(hex, region, pop);
    }
    
    public void CloseFlyOut()
    {
        _hrpflyout.CloseFlyOut();
    }
}
