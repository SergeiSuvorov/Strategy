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
        [Inject] private CommandCreatorBase<ISetRendezvousPointCommand> _rendevouser;

        private ICommandsQueue _queue;

        private bool _commandIsPending;

        [Inject] private Vector3Value _groundClicksRMB;

        public void OnCommandButtonClicked(ICommandExecutor commandExecutor, ICommandsQueue commandsQueue)
        {
            _groundClicksRMB.OnNewValue -= OnGroundClicksRMB;
            if (_commandIsPending)
            {
                ProcessOnCancel();
            }
            _commandIsPending = true;
            OnCommandAccepted?.Invoke(commandExecutor);

            _unitProducer.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
            _attacker.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
            _stoper.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
            _mover.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
            _patroller.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
            _rendevouser.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
        }


        public void ExecuteCommandWrapper(object command, ICommandsQueue commandsQueue)
        {
            _groundClicksRMB.OnNewValue -= OnGroundClicksRMB;
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                commandsQueue.Clear();
            }
            commandsQueue.EnqueueCommand(command);
            _commandIsPending = false;
            OnCommandSent?.Invoke();
            _groundClicksRMB.OnNewValue += OnGroundClicksRMB;
        }

        public void OnSelectionChanged()
        {
            _commandIsPending = false;
            ProcessOnCancel();
        }


        private void OnGroundClicksRMB(Vector3 argument)
        {
            Debug.Log("Нажата правая кнопка мыши");
            ExecuteCommandWrapper(new MoveCommand(argument), _queue);
        }

        public void OnSelectionChanged(ICommandsQueue queue)
        {
            _groundClicksRMB.OnNewValue -= OnGroundClicksRMB;
            _commandIsPending = false;

            _queue = queue;
            ProcessOnCancel();
            _groundClicksRMB.OnNewValue += OnGroundClicksRMB;
        }

        private void ProcessOnCancel()
        {
            _unitProducer.ProcessCancel();
            _attacker.ProcessCancel();
            _stoper.ProcessCancel();
            _mover.ProcessCancel();
            _patroller.ProcessCancel();
            _rendevouser.ProcessCancel();

            OnCommandCancel?.Invoke();
        }

    }
}