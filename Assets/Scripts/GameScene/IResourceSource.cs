using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceSource
{
    public int Capacity { get; }
    public int Count { get; }
    public ResourceBundle PreviewAvailableResources();
    public ResourceBundle Extract(ResourceBundle request);
    public void RegenerateSource();
}
