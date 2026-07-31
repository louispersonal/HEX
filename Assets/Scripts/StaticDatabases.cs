using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class StaticDatabases : MonoBehaviour
{
    [SerializeField] private Database<ResourceID, ResourceDefinition> _resourceDatabase;
    
    public Database<ResourceID, ResourceDefinition> ResourceDatabase => _resourceDatabase;
    
    [SerializeField] private Database<AnimalArchetypeID, AnimalArchetypeDefinition> _animalArchetypeDatabase;
    
    public Database<AnimalArchetypeID, AnimalArchetypeDefinition> AnimalArchetypeDatabase  => _animalArchetypeDatabase;
    
    [SerializeField] private Database<SpeciesID, SpeciesDefinition>[] _speciesDatabases;

    public Database<SpeciesID, SpeciesDefinition> GetSpeciesDatabase(Biome biome)
    {
        return _speciesDatabases[(int)biome];
    }
}
