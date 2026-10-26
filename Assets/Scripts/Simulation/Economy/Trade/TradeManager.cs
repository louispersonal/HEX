using System.Collections;
using System.Collections.Generic;
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
}