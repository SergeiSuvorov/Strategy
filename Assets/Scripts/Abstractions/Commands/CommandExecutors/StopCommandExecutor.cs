using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;

namespace Core
{
    public class StopCommandExecutor : CommandExecutorBase<IStopCommand>
    {
        public override void ExecuteSpecificCommand(IStopCommand command)
        {
            Debug.Log("Cancle All Action ");
        }
    }
}