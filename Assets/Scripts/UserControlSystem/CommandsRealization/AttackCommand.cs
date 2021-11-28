using Abstractions;
using Abstractions.Commands.CommandsInterfaces;
using System.Collections;
using System.Collections.Generic;

public class AttackCommand : IAttackCommand
{
    private readonly IAttackable _attackTarget;
    public IAttackable AttackTarget => _attackTarget;
    public AttackCommand(IAttackable attackTarget)
    {
        _attackTarget = attackTarget;
    }
}
