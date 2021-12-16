using Abstractions.Commands.CommandsInterfaces;

namespace Core
{
    public class AutoAttackCommand : IAttackCommand
    {
        public IAttackable AttackTarget { get; }
        public AutoAttackCommand(IAttackable target)
        {
            AttackTarget = target;
        }
    }
}