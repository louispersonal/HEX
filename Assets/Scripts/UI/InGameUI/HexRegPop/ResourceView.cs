using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour
{
    [SerializeField] private ResourcePill _resourcePillPrefab;
    
    [SerializeField] private VerticalLayoutGroup _content;
    
    private List<ResourcePill> _activePills = new();
    
    public void Populate(Pop pop)
    {
        ClearAll();
        foreach (KeyValuePair<ResourceID, float> resource in pop.ResourceStockpile)
        {
            var pill = Instantiate(_resourcePillPrefab, _content.transform);
            _activePills.Add(pill);
            pill.Initialize(resource.Key, resource.Value);
        }
    }

    private void ClearAll()
    {
        for (int i = _activePills.Count - 1; i >= 0; i--)
        {
            Destroy(_activePills[i].gameObject);
            _activePills.RemoveAt(i);
        }
    }
}
