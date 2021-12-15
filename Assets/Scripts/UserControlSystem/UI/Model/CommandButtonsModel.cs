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
        [Inject] private AttackableValue _attackClicksRMB;

        public void OnCommandButtonClicked(ICommandExecutor commandExecutor, ICommandsQueue commandsQueue)
        {
            UnsubscribeFromValueChange();
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
            UnsubscribeFromValueChange();
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                commandsQueue.Clear();
            }
            commandsQueue.EnqueueCommand(command);
            _commandIsPending = false;
            OnCommandSent?.Invoke();
            SubscribeFromValueChange();
        }

        public void OnSelectionChanged()
        {
            _commandIsPending = false;
            ProcessOnCancel();
        }


        private void OnGroundClicksRMB(Vector3 argument)
        {
            Debug.Log("Нажата правая кнопка мыши - движение");
            ExecuteCommandWrapper(new MoveCommand(argument), _queue);
        }

        private void OnAttackClicksRMB(IAttackable argument)
        {
            Debug.Log("Нажата правая кнопка мыши - атака");
            ExecuteCommandWrapper(new AttackCommand(argument), _queue);
        }

        public void OnSelectionChanged(ICommandsQueue queue)
        {
            UnsubscribeFromValueChange();
            _commandIsPending = false;

            _queue = queue;
            ProcessOnCancel();
            SubscribeFromValueChange();
        }

        private void UnsubscribeFromValueChange()
        {
            _groundClicksRMB.OnNewValue -= OnGroundClicksRMB;
            _attackClicksRMB.OnNewValue -= OnAttackClicksRMB;
        }

        private void SubscribeFromValueChange()
        {
            _groundClicksRMB.OnNewValue += OnGroundClicksRMB;
            _attackClicksRMB.OnNewValue += OnAttackClicksRMB;
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