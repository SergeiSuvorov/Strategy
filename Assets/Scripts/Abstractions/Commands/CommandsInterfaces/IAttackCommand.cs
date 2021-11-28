namespace Abstractions.Commands.CommandsInterfaces
{
    public interface IAttackCommand : ICommand
    {
        IAttackable AttackTarget { get; }
    }
}