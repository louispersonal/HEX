using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpeciesDefinition : IDatabaseItem<SpeciesID>
{
    [SerializeField] private SpeciesID _id;
    
    public SpeciesID Id => _id;
    
    public AnimalArchetypeID ArchetypeId;
    public string SpeciesName;
    public List<AnimalTags> Tags;
    public Sprite Thumbnail;
}