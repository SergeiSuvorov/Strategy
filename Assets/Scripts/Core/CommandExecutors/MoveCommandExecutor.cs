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
        public override async Task ExecuteSpecificCommand(IMoveCommand command)
        {
            _agent.speed = _speed;
            _agent.destination = command.Target;
            _animator.SetTrigger("Walk");
            _stopCommand.CancellationToken = new CancellationTokenSource();
            try
            {
                await _stop.WithCancellation(_stopCommand.CancellationToken.Token);
            }
            catch
            {
                _agent.Stop();
            }
            _stopCommand.CancellationToken = null;

            _animator.SetTrigger("Idle");
        }
    }
}