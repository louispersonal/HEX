using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class HexVegetationSource : IResourceSource
{
    public TickableType TickableType => TickableType.Simulator;

    public AxialCoordinate Coord {get; private set;}
    
    public Hex AssociatedHex => GameController.Instance.SessionManager.WorldData.Grid.GetHex(Coord);
    
    private List<ResourceRequest> _pendingRequests = new List<ResourceRequest>();
    
    public float HighVegetation => AssociatedHex.ExtraData.HighVegetation;
    public float LowVegetation => AssociatedHex.ExtraData.LowVegetation;

    public HexVegetationSource(Hex hex)
    {
        Coord = hex.Coord;
    }
    
    // Resource Source
    public ResourcePreview PreviewAvailableResources()
    {
        var highVegAbundances = VegetationProfiles.Profiles[AssociatedHex.ExtraData.Biome]
            .HighVegetationProfile.Abundances;

        float highVegAbundanceConversionFactor = 1 * HighVegetation;

        var availableContents = new ResourceCollection();
        foreach (var abundance in highVegAbundances)
        {
            availableContents.Deposit(abundance.ResourceId,
                abundance.RelativeAbundance * highVegAbundanceConversionFactor);
        }

        var lowVegAbundances = VegetationProfiles.Profiles[AssociatedHex.ExtraData.Biome]
            .LowVegetationProfile.Abundances;

        float lowVegAbundanceConversionFactor = 1 * LowVegetation;

        foreach (var abundance in lowVegAbundances)
        {
            availableContents.Deposit(abundance.ResourceId,
                abundance.RelativeAbundance * lowVegAbundanceConversionFactor);
        }
        
        return new ResourcePreview(availableContents);
    }

    public void AddExtractRequest(ResourceRequest request)
    {
        _pendingRequests.Add(request);
    }
    
    public void RegenerateSource()
    {
        // vegetation isn't stockpiled
    }
    
    public void ResolveRequests()
    {
        ResourceCollection productionSnapshot = PreviewAvailableResources().Contents;
        _pendingRequests.Shuffle();
        foreach (var request in _pendingRequests)
        {
            var deliveryContents = new ResourceCollection();
            
            bool completeFulfilment = true;
            foreach (ResourceID resource in request.Contents.GetAllResourceIDs())
            {
                completeFulfilment &= productionSnapshot.Withdraw(resource,
            request.Contents.Get(resource), out var amountRemoved);
                
                deliveryContents.Deposit(resource, amountRemoved);
            }
        
            request.Destroy();
        
            var delivery = new ResourceDelivery(deliveryContents, null);
            
            request.Sender.Stockpile.ReceiveDelivery(delivery);

            if (!completeFulfilment) break;
        }
        
        _pendingRequests.Clear();
    }

    public void Tick(TickInfo tickInfo)
    {
        ResolveRequests();
        RegenerateSource();
    }
}
