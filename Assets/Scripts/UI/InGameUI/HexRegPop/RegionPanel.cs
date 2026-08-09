using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionPanel : Panel
{
    [SerializeField] private AnimalPill _animalPillPrefab;
    
    [SerializeField] private VerticalLayoutGroup _content;
    
    public void Populate(Region region)
    {
        foreach (var species in region.Animals.Keys)
        {
            SpeciesDefinition def = 
                GameController.Instance.StaticDatabases.GetSpeciesDatabase(region.Biome).Get(species);
            var pill = Instantiate(_animalPillPrefab, _content.transform);
            pill.Initialize(def);
        }
    }
}
