using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using System.Threading.Tasks;

namespace Core
{
    public class PatrolCommandExecutor : CommandExecutorBase<IPatrolCommand>
    {
        public override async Task ExecuteSpecificCommand(IPatrolCommand command)
        {
            Debug.Log("Patroling ");
        }
    }
}