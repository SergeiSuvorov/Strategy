using System.Threading.Tasks;

namespace Abstractions.Commands
{
    public interface ICommandExecutor
    {
        Task TryExecuteCommand(object command);
    }

    public interface ICommandExecutor<T> : ICommandExecutor where T : ICommand
    {
    }

}