using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalPill : MonoBehaviour
{
    [SerializeField] private Image _thumbnail;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _size;
    [SerializeField] private TextMeshProUGUI _diet;
    [SerializeField] private TextMeshProUGUI _population;
    [SerializeField] private TextMeshProUGUI _tags;

    public void Initialize(Region region, SpeciesID species)
    {
        SpeciesDefinition def = GameController.Instance.StaticDatabases.SpeciesDatabase.Get(species);
        AnimalArchetypeDefinition archetype =
            GameController.Instance.StaticDatabases.AnimalArchetypeDatabase.Get(def.ArchetypeId);
        
        _thumbnail.sprite = def.Thumbnail;
        _name.text = def.SpeciesName;
        _size.text = archetype.SizeString();
        _diet.text = GetDietString(archetype);
        _population.text = region.Animals[species].ToString();
        _tags.text = def.GetTagsString();
    }

    private string GetDietString(AnimalArchetypeDefinition archetype)
    {
        string resources = "";
        foreach (var resourceId in archetype.Diet)
        {
            string name =
                GameController.Instance.StaticDatabases.ResourceDatabase.Get(resourceId).DisplayName;
            resources += name + " ";
        }
        return resources;
    }
}
