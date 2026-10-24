using System;
using UnityEngine;

[Serializable]
public class PersonOfInterest
{
    [SerializeField] private string _name;
    public string Name => _name;
    [SerializeField] private PersonOfInterestRole _role;
    public PersonOfInterestRole Role => _role;

    public PersonOfInterest(string name, PersonOfInterestRole role)
    {
        _name = name;
        _role = role;
    }
}

public enum PersonOfInterestRole
{
    Leader,
    Explorer
}