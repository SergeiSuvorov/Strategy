using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using System.Threading;

namespace Core
{
    public class StopCommandExecutor : CommandExecutorBase<IStopCommand>
    {
        public CancellationTokenSource CancellationToken;
        public override void ExecuteSpecificCommand(IStopCommand command)
        {
            Debug.Log("Cancle All Action ");
            CancellationToken?.Cancel();
        }
    }
}