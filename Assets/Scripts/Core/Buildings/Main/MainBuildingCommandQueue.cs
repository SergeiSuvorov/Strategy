using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainBuildingCommandQueue : MonoBehaviour, ICommandsQueue

{
    [Inject] CommandExecutorBase<IProduceUnitCommand> _produceUnitCommandExecutor;
    [Inject] CommandExecutorBase<ISetRendezvousPointCommand> _setRendezvousPointCommandExecutor;
    public ICommand CurrentCommand => default;

    public void Clear() { }

    public async void EnqueueCommand(object command)
    {
        Debug.Log("MainBuildingCommandQueue");
        await _produceUnitCommandExecutor.TryExecuteCommand(command);
        await _setRendezvousPointCommandExecutor.TryExecuteCommand(command);
    }

}
