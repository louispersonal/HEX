using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcePill : MonoBehaviour
{
    [SerializeField] private Image _thumbnail;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _quantity;
    
    public void Initialize(ResourceID resource, float quantity)
    {
        ResourceDefinition def = GameController.Instance.StaticDatabases.ResourceDatabase.Get(resource);
        
        _thumbnail.sprite = def.Thumbnail;
        _name.text = def.DisplayName;
        _quantity.text = quantity.ToString("0.#");
    }
}
