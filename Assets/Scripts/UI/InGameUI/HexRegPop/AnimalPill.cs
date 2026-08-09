using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalPill : MonoBehaviour
{
    [SerializeField] private Image _thumbnail;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _tags;

    public void Initialize(SpeciesDefinition def)
    {
        _thumbnail.sprite = def.Thumbnail;
        _name.text = def.SpeciesName;
        _tags.text = def.GetTagsString();
    }
}
