using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;

namespace Core
{
    public class PatrolCommand : CommandExecutorBase<IPatrolCommand>
    {
        public override void ExecuteSpecificCommand(IPatrolCommand command)
        {
            Debug.Log("Patroling ");
        }
    }
}