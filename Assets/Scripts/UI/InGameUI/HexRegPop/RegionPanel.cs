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
    
    public void Initialize(Region region)
    {
        Initialized = true;
        _region = region;
        UpdatePanel();
    }

    private void ClearAll()
    {
        for (int i = _activePills.Count - 1; i >= 0; i--)
        {
            Destroy(_activePills[i].gameObject);
            _activePills.RemoveAt(i);
        }
    }

    public void UpdatePanel()
    {
        ClearAll();
        foreach (var species in _region.Animals.Keys)
        {
            var pill = Instantiate(_animalPillPrefab, _content.transform);
            _activePills.Add(pill);
            pill.Initialize(_region, species);
        }
    }

    public void Terminate()
    {
        Initialized = false;
    }
}
