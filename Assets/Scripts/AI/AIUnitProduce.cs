using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserControlSystem;
using Zenject;

public class AIUnitProduce 
{
    [Inject] private CommandCreatorBase<IProduceUnitCommand> _unitProducer;
    private CommandExecutorBase<IProduceUnitCommand> _chomperProduceUnitCommandExecute;
    private CommandExecutorBase<IProduceUnitCommand> _spitterProduceUnitCommandExecute;
   
    public void SetChomperProducer(CommandExecutorBase<IProduceUnitCommand> chomperProduceUnitCommandExecute)
    {
        if(_chomperProduceUnitCommandExecute ==null
            || chomperProduceUnitCommandExecute==null)
        _chomperProduceUnitCommandExecute = chomperProduceUnitCommandExecute;
    }

    public void SetSpitterProducer(CommandExecutorBase<IProduceUnitCommand> spitterProduceUnitCommandExecute)
    {
        if (_spitterProduceUnitCommandExecute == null
            || spitterProduceUnitCommandExecute == null)
            _spitterProduceUnitCommandExecute = spitterProduceUnitCommandExecute;
    }

    public void CreateUnit(UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Chomper:
                if (_chomperProduceUnitCommandExecute != null)
                    _unitProducer.ProcessCommandExecutor(_chomperProduceUnitCommandExecute, command => _chomperProduceUnitCommandExecute.ExecuteSpecificCommand(command));
                break;
            case UnitType.ChomperMod:
                if (_chomperProduceUnitCommandExecute != null)
                    _unitProducer.ProcessCommandExecutor(_chomperProduceUnitCommandExecute, command => _chomperProduceUnitCommandExecute.ExecuteSpecificCommand(command));
                break;
            case UnitType.Spitter:
                
                    if (_spitterProduceUnitCommandExecute != null)
                        _unitProducer.ProcessCommandExecutor(_spitterProduceUnitCommandExecute, command => _spitterProduceUnitCommandExecute.TryExecuteCommand(command));
                    break;
            default:
                if (_chomperProduceUnitCommandExecute != null)
                    _unitProducer.ProcessCommandExecutor(_chomperProduceUnitCommandExecute, command => _chomperProduceUnitCommandExecute.ExecuteSpecificCommand(command));
                break;
        }
    }
}
