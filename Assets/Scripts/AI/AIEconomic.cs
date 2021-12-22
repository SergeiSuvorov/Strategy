using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class AIEconomic
{
    [Inject(Id = "Fraction ID")] private int _factionId;
    public event Action NeedMoneyProduce;
    public event Action CanSpendMoney;

    [Inject]
    private void Init(IShowFactionMoney showFaction)
    {
        showFaction.FactionMoney.ObserveReplace().Subscribe(replaceEvent =>
        {
            if (replaceEvent.Key == _factionId)
            {
                CheckMoney(replaceEvent.NewValue);
            }
        }
        );
    }
    public void CheckMoney(int moneyValue)
    {
        if (!AIRegister.MoneyFactoryDictionary.ContainsKey(_factionId) || AIRegister.MoneyFactoryDictionary[_factionId].Count < 1)
        {
            NeedMoneyProduce?.Invoke();
        }
        else if (moneyValue>300)
            CanSpendMoney?.Invoke();
    }
}
