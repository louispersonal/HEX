using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexVegetationSource : IResourceSource
{
    public TickableType TickableType => TickableType.Simulator;

    public AxialCoordinate Coord {get; private set;}
    
    public Hex AssociatedHex => GameController.Instance.SessionManager.WorldData.Grid.GetHex(Coord);
    
    private List<ResourceRequest> _pendingRequests = new List<ResourceRequest>();
    
    public float HighVegetation => AssociatedHex.ExtraData.HighVegetation;
    public float LoqVegetation => AssociatedHex.ExtraData.LowVegetation;

    public HexVegetationSource(Hex hex)
    {
        Coord = hex.Coord;
    }
    
    // Resource Source
    public ResourcePreview PreviewAvailableResources()
    {
        var abundances = VegetationProfiles.Profiles[AssociatedHex.ExtraData.Biome]
            .HighVegetationProfile.Abundances;

        float abundanceConversionFactor = 1;

        var availableContents = new ResourceCollection();
        foreach (var abundance in abundances)
        {
            availableContents.Deposit(abundance.ResourceId,
                abundance.RelativeAbundance * abundanceConversionFactor);
        }

        return new ResourcePreview(availableContents);
    }

    public void AddExtractRequest(ResourceRequest request)
    {
        _pendingRequests.Add(request);
    }

    public void SpawnSource()
    {
        
    }
    
    public void RegenerateSource()
    {
        
    }

    public void ResolveRequests()
    {
        
    }

    public void Tick(TickInfo tickInfo)
    {
        RegenerateSource();
    }
}
