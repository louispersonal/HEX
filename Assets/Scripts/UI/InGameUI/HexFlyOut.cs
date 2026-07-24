using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexFlyOut : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceText;

    public void RefreshText(HexData selectedHex)
    {
        Dictionary<ResourceID, float> resourceBuffer = new();
        GameController.Instance.SessionManager.WorldData.GetAvailableResources(selectedHex.Coord, resourceBuffer);

        string text = "Resources:\n";
        
        foreach (var resource in resourceBuffer)
        {
            string resourceName = GameController.Instance.SessionManager.WorldData.ResourceDatabase.Get(resource.Key)
                .DisplayName;
            text += resourceName + ", " + resource.Value + "\n";
        }
        
        _resourceText.text = text;
    }
}
