using System;
using UnityEngine;

[Serializable]
public class ResourceDefinition : IDatabaseItem<ResourceID>
{
    [SerializeField] private ResourceID _id;
    
    public ResourceID Id => _id;
    public string DisplayName;
    public Sprite Thumbnail;
    
    public ResourceID ParentID;
    public bool IsAbstract;
    
    public bool IsSpeciesResource;
    public SpeciesID SourceSpeciesID;
    public string SpeciesProductKey;
}