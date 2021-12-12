using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using UnityEngine.AI;
using System.Threading;
using Utils;
using System.Threading.Tasks;

namespace Core
{
    public class MoveCommandExecutor : CommandExecutorBase<IMoveCommand>
    {
        [SerializeField] private float _speed;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private UnitMovementStop _stop;
        [SerializeField] private StopCommandExecutor _stopCommand;

        public float Speed => _speed;
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Idle = Animator.StringToHash("Idle");

        public override async Task ExecuteSpecificCommand(IMoveCommand command)
        {
            _agent.speed = _speed;
            _agent.destination = command.Target;
            _agent.stoppingDistance = 0.1f;
            _animator.SetTrigger(Walk);
            _stopCommand.CancellationToken = new CancellationTokenSource();
            try
            {
                await _stop
                    .WithCancellation
                    (
                        _stopCommand
                            .CancellationToken
                            .Token
                    );
            }
            catch
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }
            _stopCommand.CancellationToken = null;
            Debug.Log("Stop");
            _animator.SetTrigger(Idle);
        }
    }
}