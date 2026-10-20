using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionPanel : Panel
{
    [SerializeField] private AnimalPill _animalPillPrefab;
    
    [SerializeField] private VerticalLayoutGroup _content;

    private List<AnimalPill> _activePills = new();

    private Region _region;

    public void SetData(Region region)
    {
        _region = region;
    }
    
    private void Clear()
    {
        for (int i = _activePills.Count - 1; i >= 0; i--)
        {
            Destroy(_activePills[i].gameObject);
            _activePills.RemoveAt(i);
        }
    }

    public void UpdatePanel()
    {
        if (_region == null)
        {
            Clear();
            return;
        }
        
        Clear();
        foreach (var species in _region.Animals.Keys)
        {
            var pill = Instantiate(_animalPillPrefab, _content.transform);
            _activePills.Add(pill);
            pill.Initialize(_region, species);
        }
    }
}
