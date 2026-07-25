using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources/Resource Database")]
public sealed class ResourceDatabase : Database<ResourceID, ResourceDefinition>
{
}