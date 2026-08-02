using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AnimalArchetypeDefinition : IDatabaseItem<AnimalArchetypeID>
{
    [SerializeField] private AnimalArchetypeID _id;
    
    public AnimalArchetypeID Id => _id;
    
    public string CommonName;

    public ushort Size; // {0, 1, 2}

    public ResourceID[] Diet;

    [Tooltip("Or hunting ability in predators")]
    public float ForagingAbility;

    public float NutritionRequired;

    public float NutritionProvided;
}