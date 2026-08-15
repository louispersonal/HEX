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
            SpeciesDefinition def = GameController.Instance.StaticDatabases.SpeciesDatabase.Get(species);
            var pill = Instantiate(_animalPillPrefab, _content.transform);
            _activePills.Add(pill);
            pill.Initialize(def);
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
