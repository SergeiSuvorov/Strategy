using Abstractions.Commands.CommandsInterfaces;
using UserControlSystem;

public class ConquerCommandCommandCreator : CancellableCommandCreatorBase<IConquerCommand, IConquerable>
{
    protected override IConquerCommand CreateCommand(IConquerable argument) => new ConquerCommand(argument);
}
