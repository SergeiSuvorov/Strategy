using Abstractions;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Core.CommandExecutors;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using Zenject;

public class AIManager : MonoBehaviour
{
    [Inject] private AIEconomic _aIEconomic;
    //[Inject] private AIUnitProduce _aIUnitProduce;
    //[Inject(Id = "Fraction ID")] private int _factionId;
    //[Inject(Id = "Max Unit Count")] private int _maxUnitCount;
    //private int _enemyFactionId;
    //private bool _isAttack;
    [Inject] private AIAttackModel _aIAttackModel;
    [Inject] private AIUnitManager _aIUnitManager;
   
    //private List<CommandExecutorBase<IAttackCommand>> _attackUnitList =new List<CommandExecutorBase<IAttackCommand>>();
    //private List<CommandExecutorBase<IConquerCommand>> _conqeurUnitList = new List<CommandExecutorBase<IConquerCommand>>();


    private void Awake()
    {
        var attackUnitList = new List<CommandExecutorBase<IAttackCommand>>();
        var conqeurUnitList = new List<CommandExecutorBase<IConquerCommand>>();

        _aIAttackModel.Init(attackUnitList, conqeurUnitList);
        _aIUnitManager.Init(attackUnitList, conqeurUnitList);
        _aIEconomic.NeedMoneyProduce += _aIAttackModel.StartConqeur;
        _aIEconomic.CanSpendMoney += _aIAttackModel.CheckArmyReady;
        _aIAttackModel.NeedAttackUnit += _aIUnitManager.CreateArmy;
        _aIAttackModel.NeedConqeureUnit += _aIUnitManager.CreateProduceConqeur;
        _aIUnitManager.AttackUnitIsReady += _aIAttackModel.CalculateAttackReadiness;
        _aIUnitManager.ConqeureUnitIsReady += _aIAttackModel.SetConqeureGoal;

       

        //_enemyFactionId = AIRegister.EnemyID;

        //AIRegister.UnitDictionary[_factionId].ObserveAdd().Subscribe(record =>
        //{
        //    AddUnit(record.Value);
        //});

        //AIRegister.UnitDictionary[_factionId].ObserveRemove().Subscribe(record =>
        //{
        //    RemoveUnit(record.Value);
        //});

        //AIRegister.UnitDictionary[_enemyFactionId].ObserveRemove().Subscribe(record =>
        //{
        //    if (_isAttack)
        //    {
        //        SetDestroyGoal();
        //    }
        //});

        //AIRegister.BuildingDictionary[_enemyFactionId].ObserveRemove().Subscribe(record =>
        //{
        //    if (_isAttack)
        //    {
        //        SetDestroyGoal();
        //    }
        //});

        //var buildingList = AIRegister.BuildingDictionary[_factionId];
        //for(int i=0; i<buildingList.Count; i++)
        //{
        //    buildingList[i].TryGetComponent(out IUnitTypeCreater building);
        //    if (building.UnitType == UnitType.Chomper)
        //    {
        //        var chomperProduceUnitCommandExecute = (building as Component).GetComponent<ProduceUnitCommandExecutor>();
        //        _aIUnitProduce.SetChomperProducer(chomperProduceUnitCommandExecute);
        //    }
        //    if (building.UnitType == UnitType.Spitter)
        //    {
        //        var spitterProduceUnitCommandExecute = (building as Component).GetComponent<ProduceUnitCommandExecutor>();
        //        _aIUnitProduce.SetSpitterProducer(spitterProduceUnitCommandExecute);
        //    }
        //}

    }

   
    //private void StartConqeur()
    //{
    //    if (_conqeurUnitList .Count < 1)
    //        CreateProduceConqeur();
    //    else
    //        SetConqeureGoal();
    //}

    //private void CreateArmy()
    //{
    //    if (_attackUnitList.Count < _maxUnitCount)
    //    {
    //        _isAttack = false;
    //        _aIUnitProduce.CreateUnit(UnitType.Chomper);
    //    }
    //    else if(!_isAttack)
    //        CalculateAttackReadiness();
    //}
    //private void CalculateAttackReadiness()
    //{
    //    var enemyUnit = AIRegister.UnitDictionary[_enemyFactionId];
    //    var randomIndex = UnityEngine.Random.Range(-6, 6);
    //    if(enemyUnit.Count< _attackUnitList.Count+randomIndex)
    //    {
    //        SetDestroyGoal();
    //    }
    //    else
    //        Debug.Log("NotYet");
    //}
    //private void SetDestroyGoal()
    //{
    //    _isAttack = true;
    //    ReactiveCollection<Transform> attackObjectList = new ReactiveCollection<Transform>();
    //    if (AIRegister.BuildingDictionary[_enemyFactionId].Count != 0)
    //    {
    //        attackObjectList = AIRegister.BuildingDictionary[_enemyFactionId];
    //    }
    //    else if(AIRegister.UnitDictionary[_enemyFactionId].Count != 0)
    //    {
    //        attackObjectList = AIRegister.UnitDictionary[_enemyFactionId];
    //    }
    //    else
    //    {
    //        _isAttack = false;
    //        StartConqeur();
    //        return;
    //    }
    //    var attackObject = ChooseObjectForDestroy(attackObjectList);
    //    if (attackObject == null)
    //        return;
    //    var attackCommand = new AttackCommand(attackObject);
    //    for (int i = 0; i < _attackUnitList.Count; i++)
    //    {
    //        _attackUnitList[i].TryExecuteCommand(attackCommand);
    //    }
    //}

    //private IAttackable ChooseObjectForDestroy(ReactiveCollection<Transform> enemys)
    //{
    //    var index = UnityEngine.Random.Range(0, enemys.Count);
    //    if (enemys[index] != null)
    //    {
    //        var attackObject = enemys[index].GetComponent<IAttackable>();
    //        return attackObject;
    //    }
    //    else return null;
    
    //}
    //private void SetConqeureGoal()
    //{
    //    var freeMoneyFactoryList = AIRegister.MoneyFactoryDictionary[0];
    //    var count = _conqeurUnitList .Count > freeMoneyFactoryList.Count ? freeMoneyFactoryList.Count : _conqeurUnitList .Count;
    //    for (int i=0;i<count; i++)
    //    {
    //        var maxIndex = freeMoneyFactoryList.Count;
    //        var index = UnityEngine.Random.Range(0, maxIndex);
    //        freeMoneyFactoryList[index].TryGetComponent(out IConquerable conquerable);
    //        var conqeurGoal = new ConquerCommand(conquerable);
    //        _conqeurUnitList[i].TryExecuteCommand(conqeurGoal);
    //    }
    //}
    //private void CreateProduceConqeur()
    //{
    //    _aIUnitProduce.CreateUnit(UnitType.Spitter);
    //}

    //private void AddUnit(Transform unit)
    //{
    //    if (unit.TryGetComponent(out CommandExecutorBase<IConquerCommand>  conquerUnit))
    //    {
    //        _conqeurUnitList .Add(conquerUnit);
    //        SetConqeureGoal();
    //    }
    //    else
    //    {
    //        CommandExecutorBase<IAttackCommand> attackExecutor = unit.GetComponent<CommandExecutorBase<IAttackCommand>>();
    //        _attackUnitList.Add(attackExecutor);
    //        CalculateAttackReadiness();
    //    }
    //}

    //private void RemoveUnit(Transform unit)
    //{
    //    CommandExecutorBase<IAttackCommand> attackExecutor = unit.GetComponent<CommandExecutorBase<IAttackCommand>>();
    //    if ((unit.TryGetComponent(out CommandExecutorBase<IConquerCommand> conquerUnit) && _conqeurUnitList .Contains(conquerUnit)))
    //        _conqeurUnitList .Remove(conquerUnit);
    //    else if (_attackUnitList.Contains(attackExecutor))
    //        _attackUnitList.Remove(attackExecutor);
    //}
}
