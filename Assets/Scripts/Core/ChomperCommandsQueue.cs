using UnityEngine;
using UniRx;
using Zenject;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UserControlSystem.CommandsRealization;

public class ChomperCommandsQueue : MonoBehaviour, ICommandsQueue
{
    [Inject] CommandExecutorBase<IMoveCommand> _moveCommandExecutor;
    [Inject] CommandExecutorBase<IPatrolCommand> _patrolCommandExecutor;
    [Inject] CommandExecutorBase<IAttackCommand> _attackCommandExecutor;
    [Inject] CommandExecutorBase<IStopCommand> _stopCommandExecutor;

    private ReactiveCollection<ICommand> _innerCollection = new ReactiveCollection<ICommand>();

    [Inject] private void Init()
    {
        _innerCollection.ObserveAdd().Subscribe(OnNewCommand).AddTo(this);
    }

    private void OnNewCommand(ICommand command, int index)
    {
        Debug.Log(index);
        Debug.Log(command.GetType().ToString());
        if (index == 0)
            ExecuteCommand(command);
    }
    public void Clear()
    {
        _innerCollection.Clear();
        _stopCommandExecutor.ExecuteSpecificCommand(new StopCommand());

    }

    private async void ExecuteCommand(ICommand command)
    {
        await _moveCommandExecutor.TryExecuteCommand(command);
        await _patrolCommandExecutor.TryExecuteCommand(command);
        await _attackCommandExecutor.TryExecuteCommand(command);
        await _stopCommandExecutor.TryExecuteCommand(command);
        if (_innerCollection.Count > 0)
        {
            _innerCollection.RemoveAt(0);
        }
        CheckTheQueue();
    }

    private void CheckTheQueue()
    {
        Debug.Log(_innerCollection.Count);
        
        if (_innerCollection.Count > 0)
        {
            Debug.Log(_innerCollection[0].GetType().ToString());
            ExecuteCommand(_innerCollection[0]);
        }
    }

    public void EnqueueCommand(object wrappedCommand)
    {
        var command = wrappedCommand as ICommand;
        _innerCollection.Add(command);
    }


}
