using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Resource Database")]
public sealed class ResourceDatabase : Database<ResourceID, ResourceDefinition>
{
    public bool IsA(ResourceID child, ResourceID parent)
    {
        ResourceID current = child;

        while (TryGet(current, out ResourceDefinition definition))
        {
            if (current == parent)
                return true;

            if (definition.ParentID == null)
                return false;

            current = definition.ParentID.Value;
        }

        return false;
    }
}