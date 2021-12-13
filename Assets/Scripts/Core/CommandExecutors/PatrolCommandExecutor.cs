using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using System.Threading.Tasks;
using UnityEngine.AI;
using UserControlSystem.CommandsRealization;
using System.Threading;
using Utils;

namespace Core
{
    public class PatrolCommandExecutor : CommandExecutorBase<IPatrolCommand>
    {
        [SerializeField] private UnitMovementStop _stop;
        [SerializeField] private StopCommandExecutor _stopCommand;
        [SerializeField] private MoveCommandExecutor _move;

        public override async Task ExecuteSpecificCommand(IPatrolCommand command)
        {
            bool isPatrol = true;
            var _nextPatrolPoint = gameObject.transform.position;
            Debug.Log("Patroling  from" + command.From + " to " + command.To);
            _stopCommand.CancellationToken = new CancellationTokenSource();
            while (isPatrol)
            {
                if (_nextPatrolPoint == command.To)
                    _nextPatrolPoint = command.From;
                else _nextPatrolPoint = command.To;

                _move.ExecuteSpecificCommand(new MoveCommand(_nextPatrolPoint));
                try
                {
                    await _stop.WithCancellation(_stopCommand.CancellationToken.Token);
                }
                catch
                {

                    break;
                }

                Debug.Log("nextPatrolPoint");
            }
        }
    }
}