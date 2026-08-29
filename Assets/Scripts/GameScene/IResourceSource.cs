using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceSource
{
    public ResourceBundle GetAvailableResources();
    public ResourceBundle Extract(ResourceBundle request);
}
