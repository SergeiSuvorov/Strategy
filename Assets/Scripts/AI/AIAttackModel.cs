using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using Zenject;
using System;

public class AIAttackModel
{
    [Inject(Id = "Max Unit Count")] private int _maxUnitCount;
    private int _enemyFactionId;
    public bool _isAttack;

    private List<CommandExecutorBase<IAttackCommand>> _attackUnitList = new List<CommandExecutorBase<IAttackCommand>>();
    private List<CommandExecutorBase<IConquerCommand>> _conqeurUnitList = new List<CommandExecutorBase<IConquerCommand>>();

    public event Action NeedAttackUnit;
    public event Action NeedConqeureUnit;

    public void Init(List<CommandExecutorBase<IAttackCommand>> attackUnitList, List<CommandExecutorBase<IConquerCommand>> conqeurUnitList)
    {
        _enemyFactionId = AIRegister.EnemyID;

        AIRegister.UnitDictionary[_enemyFactionId].ObserveRemove().Subscribe(record =>
        {
            if (_isAttack)
            {
                SetDestroyGoal();
            }
        });

        AIRegister.BuildingDictionary[_enemyFactionId].ObserveRemove().Subscribe(record =>
        {
            if (_isAttack)
            {
                SetDestroyGoal();
            }
        });

        _attackUnitList = attackUnitList;
        _conqeurUnitList = conqeurUnitList;
    }

    public void CheckArmyReady()
    {
        if (_attackUnitList.Count < _maxUnitCount)
        {
            _isAttack = false;
            NeedAttackUnit?.Invoke();
        }
        else if (!_isAttack)
            CalculateAttackReadiness();
    }

    public void StartConqeur()
    {
        if (_conqeurUnitList.Count < 1)
            NeedConqeureUnit?.Invoke();
        else
            SetConqeureGoal();
    }

    public void CalculateAttackReadiness()
    {
        var enemyUnit = AIRegister.UnitDictionary[_enemyFactionId];
        var randomIndex = UnityEngine.Random.Range(-_maxUnitCount, _maxUnitCount);
        if (enemyUnit.Count < _attackUnitList.Count + randomIndex)
        {
            SetDestroyGoal();
        }
        else
            Debug.Log("NotYet");
    }
    private void SetDestroyGoal()
    {
        _isAttack = true;
        ReactiveCollection<Transform> attackObjectList = new ReactiveCollection<Transform>();
        if (AIRegister.BuildingDictionary[_enemyFactionId].Count != 0)
        {
            attackObjectList = AIRegister.BuildingDictionary[_enemyFactionId];
        }
        else if (AIRegister.UnitDictionary[_enemyFactionId].Count != 0)
        {
            attackObjectList = AIRegister.UnitDictionary[_enemyFactionId];
        }
        else
        {
            _isAttack = false;
            StartConqeur();
            return;
        }
        var attackObject = ChooseObjectForDestroy(attackObjectList);
        if (attackObject == null)
            return;
        var attackCommand = new AttackCommand(attackObject);
        for (int i = 0; i < _attackUnitList.Count; i++)
        {
            _attackUnitList[i].TryExecuteCommand(attackCommand);
        }
    }

    private IAttackable ChooseObjectForDestroy(ReactiveCollection<Transform> enemys)
    {
        var index = UnityEngine.Random.Range(0, enemys.Count);
        if (enemys[index] != null)
        {
            var attackObject = enemys[index].GetComponent<IAttackable>();
            return attackObject;
        }
        else return null;

    }
    public void SetConqeureGoal()
    {
        var freeMoneyFactoryList = AIRegister.MoneyFactoryDictionary[0];
        var count = _conqeurUnitList.Count > freeMoneyFactoryList.Count ? freeMoneyFactoryList.Count : _conqeurUnitList.Count;
        for (int i = 0; i < count; i++)
        {
            var maxIndex = freeMoneyFactoryList.Count;
            var index = UnityEngine.Random.Range(0, maxIndex);
            freeMoneyFactoryList[index].TryGetComponent(out IConquerable conquerable);
            var conqeurGoal = new ConquerCommand(conquerable);
            _conqeurUnitList[i].TryExecuteCommand(conqeurGoal);
        }
    }
}
