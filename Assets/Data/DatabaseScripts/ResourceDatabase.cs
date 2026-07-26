using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Resource Database")]
public sealed class ResourceDatabase : Database<ResourceID, ResourceDefinition>
{
}