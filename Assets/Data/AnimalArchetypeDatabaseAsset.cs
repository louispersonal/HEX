using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Game Data/Animal Archetype Database")]
public class AnimalArchetypeDatabaseAsset : ScriptableObject
{
    [FormerlySerializedAs("Species")]
    public ArchetypeProfile[] Archetypes;
} 