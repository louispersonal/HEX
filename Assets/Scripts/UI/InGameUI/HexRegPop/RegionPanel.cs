using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionPanel : Panel
{
    [SerializeField] private AnimalPill _animalPillPrefab;
    
    [SerializeField] private VerticalLayoutGroup _content;

    private List<AnimalPill> _activePills = new();
    
    public void Populate(Region region)
    {
        ClearAll();
        foreach (var species in region.Animals.Keys)
        {
            var pill = Instantiate(_animalPillPrefab, _content.transform);
            _activePills.Add(pill);
            pill.Initialize(region, species);
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
