using Abstractions.Commands.CommandsInterfaces;

public class ConquerCommand : IConquerCommand
{
    private readonly IConquerable _conquerTarget;

    public IConquerable СonquerTarget => _conquerTarget;

    public ConquerCommand(IConquerable conquerTarget)
    {
        _conquerTarget = conquerTarget;
    }
}
