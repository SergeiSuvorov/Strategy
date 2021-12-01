using System;
using System.Collections.Generic;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using UserControlSystem.CommandsRealization;
using Zenject;

namespace UserControlSystem
{
    public sealed class CommandButtonsModel
    {
        public event Action<ICommandExecutor> OnCommandAccepted;
        public event Action OnCommandSent;
        public event Action OnCommandCancel;

        [Inject] private CommandCreatorBase<IProduceUnitCommand> _unitProducer;
        [Inject] private CancellableCommandCreatorBase<IMoveCommand, Vector3> _mover;
        [Inject] private CommandCreatorBase<IAttackCommand> _attacker;
        [Inject] private CommandCreatorBase<IPatrolCommand> _patroller;
        [Inject] private CommandCreatorBase<IStopCommand> _stoper;

        private CommandExecutorBase<IMoveCommand> _moverExecutor;

        private bool _commandIsPending;
        [Inject] private Vector3Value _groundClicksRMB;
        public void OnCommandButtonClicked(ICommandExecutor commandExecutor)
        {
            if (_commandIsPending)
            {
                processOnCancel();
            }
            _commandIsPending = true;
            OnCommandAccepted?.Invoke(commandExecutor);

            _unitProducer.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
            _mover.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
            _attacker.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
            _patroller.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
            _stoper.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
        }

        public void ExecuteCommandWrapper(ICommandExecutor commandExecutor, object command)
        {
            commandExecutor.ExecuteCommand(command);
            _commandIsPending = false;
            OnCommandSent?.Invoke();
        }

        private void OnGroundClicksRMB(Vector3 argument)
        {
            _moverExecutor.ExecuteCommand(new MoveCommand(argument));
        }

        public void OnSelectionChanged(IEnumerable<ICommandExecutor> commandExecutors)
        {
            _moverExecutor = null;
            _groundClicksRMB.OnNewValue -= OnGroundClicksRMB;
            _commandIsPending = false;
            processOnCancel();

            foreach (var currentExecutor in commandExecutors)
            {
                var moveCommand = currentExecutor as CommandExecutorBase<IMoveCommand>;
                if (moveCommand != null)
                {
                    _moverExecutor = moveCommand;
                    _groundClicksRMB.OnNewValue += OnGroundClicksRMB;
                }
            }
        }

        private void processOnCancel()
        {
            _unitProducer.ProcessCancel();
            _mover.ProcessCancel();
            _attacker.ProcessCancel();
            _patroller.ProcessCancel();
            OnCommandCancel?.Invoke();
        }
    }
}