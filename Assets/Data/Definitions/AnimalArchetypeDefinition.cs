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

    public Resources[] Eats;
}