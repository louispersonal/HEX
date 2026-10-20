using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonOfInterest
{
    public string Name { get; private set; }
    public PersonOfInterestRole Role  { get; private set; }

    public PersonOfInterest(string name, PersonOfInterestRole role)
    {
        Name = name;
        Role = role;
    }
}

public enum PersonOfInterestRole
{
    Leader,
    Explorer
}