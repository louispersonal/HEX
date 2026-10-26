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