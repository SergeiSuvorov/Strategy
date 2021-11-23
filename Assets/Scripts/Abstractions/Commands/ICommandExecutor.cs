namespace Abstractions.Commands
{
    public interface ICommandExecutor
    {
        void ExecuteCommand(object command);
    }

    public interface ICommandExecutor<T> : ICommandExecutor where T : ICommand
    {
        void ExecuteSpecificCommand(T command);
    }
}