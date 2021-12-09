using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class StopCommandExecutor : CommandExecutorBase<IStopCommand>
    {
        public CancellationTokenSource CancellationToken;
        public override Task ExecuteSpecificCommand(IStopCommand command)
        {
            Debug.Log("Cancle All Action ");
            CancellationToken?.Cancel();
            return Task.CompletedTask;
        }
    }
}