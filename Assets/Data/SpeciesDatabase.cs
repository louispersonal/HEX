using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Species Database")]
public class SpeciesDatabase : Database<SpeciesID, SpeciesDefinition>
{
    public SpeciesProfile[] Species;
}
