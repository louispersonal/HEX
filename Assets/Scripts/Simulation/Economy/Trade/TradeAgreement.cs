using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeAgreement : ISavable<TradeAgreementSaveData>
{
    private PopID _partyA;
    public PopID PartyA => _partyA;

    private PopID _partyB;
    public PopID PartyB => _partyB;

    private SerializableDictionary<ResourceID, float> _resourceFlux;
    public SerializableDictionary<ResourceID, float> ResourceFlux => _resourceFlux;

    public TradeAgreement(PopID partyA, PopID partyB)
    {
        _partyA = partyA;
        _partyB = partyB;
    }
    
    public TradeAgreement(TradeAgreementSaveData tradeAgreementSaveData)
    {
        _partyA = tradeAgreementSaveData.PartyA;
        _partyB = tradeAgreementSaveData.PartyB;
        _resourceFlux = new SerializableDictionary<ResourceID, float>(tradeAgreementSaveData.ResourceFlux);
    }

    public TradeAgreementSaveData ToSaveData()
    {
        return new TradeAgreementSaveData(this);
    }

    public void AddResource(ResourceID resourceID, float amount)
    {
        if(!_resourceFlux.TryAdd(resourceID, amount))
            throw new ArgumentException($"Resource {resourceID} has already been added");
    }

    public void RemoveResource(ResourceID resourceID)
    {
        if(!_resourceFlux.Remove(resourceID))
            throw new ArgumentException($"Resource {resourceID} is not here");
    }

    public bool ModifyOrAddAmount(ResourceID resourceID, float newAmount)
    {
        // returns true if amount was modified, false if added
        if (_resourceFlux.ContainsKey(resourceID))
        {
            _resourceFlux[resourceID] = newAmount;
            return true;
        }
        
        _resourceFlux[resourceID] = newAmount;
        return false;
    }
}

[Serializable]
public class TradeAgreementSaveData
{
    public PopID PartyA;
    public PopID PartyB;
    public SerializableDictionarySaveData<ResourceID, float> ResourceFlux;

    public TradeAgreementSaveData(TradeAgreement tradeAgreement)
    {
        PartyA = tradeAgreement.PartyA;
        PartyB = tradeAgreement.PartyB;
        ResourceFlux = new SerializableDictionarySaveData<ResourceID, float>(tradeAgreement.ResourceFlux);
    }
}