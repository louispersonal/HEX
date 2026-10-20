using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class StaticDatabases : MonoBehaviour
{
    [SerializeField] private ResourceDatabase _resourceDatabase;
    
    public ResourceDatabase ResourceDatabase => _resourceDatabase;
    
    [SerializeField] private Database<AnimalArchetypeID, AnimalArchetypeDefinition> _animalArchetypeDatabase;
    
    public Database<AnimalArchetypeID, AnimalArchetypeDefinition> AnimalArchetypeDatabase  => _animalArchetypeDatabase;
    
    [SerializeField] private Database<SpeciesID, SpeciesDefinition> _speciesDatabase;
    
    public Database<SpeciesID, SpeciesDefinition> SpeciesDatabase => _speciesDatabase;
}
