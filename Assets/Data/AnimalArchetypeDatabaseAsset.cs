using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Animal Archetype Database")]
public class AnimalArchetypeDatabaseAsset : ScriptableObject
{
    public ArchetypeProfile[] Species;
} 