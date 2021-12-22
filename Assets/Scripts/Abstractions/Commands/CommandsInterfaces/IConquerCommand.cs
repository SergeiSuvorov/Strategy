namespace Abstractions.Commands.CommandsInterfaces
{
    public interface IConquerCommand : ICommand
    {
        IConquerable СonquerTarget { get; }
    }
}