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
    [Inject] private AIAttackModel _aIAttackModel;
    [Inject] private AIUnitManager _aIUnitManager;
  
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

    }
}
