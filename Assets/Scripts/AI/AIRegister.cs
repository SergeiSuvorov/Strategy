using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;
using Abstractions;
using Core.CommandExecutors;


public class AIRegister : MonoBehaviour
{
    [Inject(Id = "Fraction ID")] private int _factionId;

    public static int EnemyID { get; private set; }

    private static ReactiveDictionary<int, ReactiveCollection<Transform>> _unitDictionary = new ReactiveDictionary<int, ReactiveCollection<Transform>>();
    private static ReactiveDictionary<int, ReactiveCollection<Transform>> _buildingDictionary = new ReactiveDictionary<int, ReactiveCollection<Transform>>();
    private static ReactiveDictionary<int, ReactiveCollection<Transform>> _moneyFactoryDictionary = new ReactiveDictionary<int, ReactiveCollection<Transform>>();

    public static IReadOnlyReactiveDictionary<int, ReactiveCollection<Transform>> MoneyFactoryDictionary => _moneyFactoryDictionary;
    public static IReadOnlyReactiveDictionary<int, ReactiveCollection<Transform>> BuildingDictionary => _buildingDictionary;
    public static IReadOnlyReactiveDictionary<int, ReactiveCollection<Transform>> UnitDictionary => _unitDictionary;

    //[Inject] private AIUnitProduce _aiUnitProduce;

    private void Awake()
    {
        var factionList = FactionMember.GetFactionList();

        for(int i=0;i< factionList.Count; i++)
        {
            if (factionList[i] != _factionId
                && factionList[i] != 0)
                EnemyID = factionList[i];
        }

        for (int i = 0; i < FactionMember.FactionsCount; i++)
        {
            var memberList = FactionMember.GetMemberFactionList(factionList[i]);
            memberList.ObserveAdd().Subscribe(obj =>
            {
                var factionId = (obj.Value.GetComponent<FactionMember>().FactionId);
                Register(factionId, obj.Value);
            });
            memberList.ObserveRemove().Subscribe(obj =>
            {
                var factionId = (obj.Value.GetComponent<FactionMember>().FactionId);
                Unregister(factionId, obj.Value);
            });

            for (int a=0;a< memberList.Count; a++)
            {
                Register(factionList[i], memberList[a]);
            }
        }

        var buildingsRoot = _buildingDictionary[_factionId][0].parent;

        for(int a = 0; a< buildingsRoot.childCount; a++)
        {
            for (int i = 0; i < FactionMember.FactionsCount;i++)
            {
               if((_buildingDictionary.ContainsKey(factionList[i]) 
                    && (!_buildingDictionary[factionList[i]].Contains(buildingsRoot.GetChild(a)))))
                {
                    Register(0, buildingsRoot.GetChild(a));
                }
                    
            }
        }

        if(!_unitDictionary.ContainsKey(_factionId))
        {
            _unitDictionary.Add(_factionId, new ReactiveCollection<Transform>());
        }
    }
    private void Register(int key, Transform value)
    {

        if (value.TryGetComponent(out IUnitTypeCreater building))
        {
            if (!_buildingDictionary.ContainsKey(key))
                _buildingDictionary.Add(key, new ReactiveCollection<Transform>());
            if (!_buildingDictionary[key].Contains(value))
                _buildingDictionary[key].Add(value);

            if (key != _factionId)
                return;

           
        }

        if (value.TryGetComponent(out IUnit unit))
        {
            if (!_unitDictionary.ContainsKey(key))
                _unitDictionary.Add(key, new ReactiveCollection<Transform>());
            if (!_unitDictionary[key].Contains(value))
                _unitDictionary[key].Add(value);
        }

        if (value.TryGetComponent(out IGenerateMoney generateMoney))
        {
            if (!_moneyFactoryDictionary.ContainsKey(key))
                _moneyFactoryDictionary.Add(key, new ReactiveCollection<Transform>());
            if (!_moneyFactoryDictionary[key].Contains(value))
                _moneyFactoryDictionary[key].Add(value);
        }
    }

    private void Unregister(int key, Transform value)
    {
        if (value.TryGetComponent(out IUnitTypeCreater building) && _buildingDictionary[key].Contains(value))
        {
            _buildingDictionary[key].Remove(value);

            if (key != _factionId)
                return;
        }

        if (value.TryGetComponent(out IUnit unit) && _unitDictionary[key].Contains(value))
        {
             _unitDictionary[key].Remove(value);
        }

        if (value.TryGetComponent(out IGenerateMoney generateMoney) && _moneyFactoryDictionary[key].Contains(value))
        {
             _moneyFactoryDictionary[key].Remove(value);
        }
    }  
}
