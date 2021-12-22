using Abstractions;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Core.CommandExecutors;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using Zenject;
using System;

public class AIUnitManager 
{
    [Inject] private AIUnitProduce _aIUnitProduce;
    [Inject(Id = "Fraction ID")] private int _factionId;
    [Inject(Id = "Max Unit Count")] private int _maxUnitCount;

    private List<CommandExecutorBase<IAttackCommand>> _attackUnitList;
    private List<CommandExecutorBase<IConquerCommand>> _conqeurUnitList;
    private int _attackUnitInProduceCount=0; 

    public event Action AttackUnitIsReady;
    public event Action ConqeureUnitIsReady;

    public void Init(List<CommandExecutorBase<IAttackCommand>> attackUnitList, List<CommandExecutorBase<IConquerCommand>> conqeurUnitList)
    {
        AIRegister.UnitDictionary[_factionId].ObserveAdd().Subscribe(record =>
        {
            AddUnit(record.Value);
        });

        AIRegister.UnitDictionary[_factionId].ObserveRemove().Subscribe(record =>
        {
            RemoveUnit(record.Value);
        });

        var buildingList = AIRegister.BuildingDictionary[_factionId];
        for (int i = 0; i < buildingList.Count; i++)
        {
            buildingList[i].TryGetComponent(out IUnitTypeCreater building);
            if (building.UnitType == UnitType.Chomper)
            {
                var chomperProduceUnitCommandExecute = (building as Component).GetComponent<ProduceUnitCommandExecutor>();
                _aIUnitProduce.SetChomperProducer(chomperProduceUnitCommandExecute);
            }
            if (building.UnitType == UnitType.Spitter)
            {
                var spitterProduceUnitCommandExecute = (building as Component).GetComponent<ProduceUnitCommandExecutor>();
                _aIUnitProduce.SetSpitterProducer(spitterProduceUnitCommandExecute);
            }
        }

        _attackUnitList = attackUnitList;
        _conqeurUnitList = conqeurUnitList;
    }

    public void CreateArmy()
    {
        if (_attackUnitList.Count + _attackUnitInProduceCount < _maxUnitCount)
        {
            Debug.Log("ReadyToProduce");
            _attackUnitInProduceCount++;
            _aIUnitProduce.CreateUnit(UnitType.Chomper);
        }
        else
            Debug.Log("NotReadyToProduce");
    }
  
    public void CreateProduceConqeur()
    {
        _aIUnitProduce.CreateUnit(UnitType.Spitter);
    }

    private void AddUnit(Transform unit)
    {
        if (unit.TryGetComponent(out CommandExecutorBase<IConquerCommand> conquerUnit))
        {
            _conqeurUnitList.Add(conquerUnit);
            ConqeureUnitIsReady?.Invoke();
        }
        else
        {
            _attackUnitInProduceCount--;
            CommandExecutorBase<IAttackCommand> attackExecutor = unit.GetComponent<CommandExecutorBase<IAttackCommand>>();
            _attackUnitList.Add(attackExecutor);
            AttackUnitIsReady?.Invoke();
        }
    }

    private void RemoveUnit(Transform unit)
    {
        CommandExecutorBase<IAttackCommand> attackExecutor = unit.GetComponent<CommandExecutorBase<IAttackCommand>>();
        if ((unit.TryGetComponent(out CommandExecutorBase<IConquerCommand> conquerUnit) && _conqeurUnitList.Contains(conquerUnit)))
            _conqeurUnitList.Remove(conquerUnit);
        else if (_attackUnitList.Contains(attackExecutor))
            _attackUnitList.Remove(attackExecutor);
    }
}
