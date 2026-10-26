using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TradeManager : ISavable<TradeAgreementSaveData[] >
{
    private List<TradeAgreement> _tradeAgreements = new();

    public TradeManager() {}

    public TradeManager(TradeAgreementSaveData[] tradeAgreementsData)
    {
        _tradeAgreements = new();
        foreach (var tradeAgreementSaveData in tradeAgreementsData)
        {
            _tradeAgreements.Add(new TradeAgreement(tradeAgreementSaveData));
        }
    }

    public TradeAgreementSaveData[] ToSaveData()
    {
        var saveData = new List<TradeAgreementSaveData>();
        foreach (var tradeAgreement in _tradeAgreements)
        {
            saveData.Add(new TradeAgreementSaveData(tradeAgreement));
        }
        return saveData.ToArray();
    }

    public bool TryFindTradeAgreement(PopID partyA, PopID partyB, out TradeAgreement agreement)
    {
        agreement = null;
        foreach (var tradeAgreement in _tradeAgreements)
        {
            if ((tradeAgreement.PartyA == partyA && tradeAgreement.PartyB == partyB) ||
                (tradeAgreement.PartyB == partyA && tradeAgreement.PartyA == partyB))
            {
                agreement = tradeAgreement;
                return true;
            }
        }

        return false;
    }
    
    public void EstablishTrade(PopID partyA, PopID partyB)
    {
        _tradeAgreements.Add(new TradeAgreement(partyA, partyB));
    }

    public bool TryAmendAgreement(PopID partyA, PopID partyB, Dictionary<ResourceID, float> newAmounts)
    {
        if (!TryFindTradeAgreement(partyA, partyB, out var agreement)) return false;
        
        foreach (KeyValuePair<ResourceID, float> amount in newAmounts)
        {
            agreement.ModifyOrAddAmount(amount.Key, amount.Value);
        }

        return false;
    }
}