using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Species Database")]
public class SpeciesDatabaseAsset : ScriptableObject
{
    public SpeciesProfile[] Species;
} 