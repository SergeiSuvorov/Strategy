using System;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UserControlSystem.CommandsRealization;

namespace UserControlSystem
{
    public sealed class StopCommandCommandCreator : CommandCreatorBase<IStopCommand>
    {
        protected override void ClassSpecificCommandCreation(ICommandExecutor commandExecutor, Action<IStopCommand> creationCallback) => creationCallback?.Invoke(new StopCommand());
      
    }
}