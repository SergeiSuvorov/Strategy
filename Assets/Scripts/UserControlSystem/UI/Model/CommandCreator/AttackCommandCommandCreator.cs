using Abstractions;
using Abstractions.Commands.CommandsInterfaces;
using System;
using System.Threading;
using UserControlSystem;
using Utils;
using Zenject;

public class AttackCommandCommandCreator : CancellableCommandCreatorBase<IAttackCommand, IAttackable>
{
    protected override IAttackCommand CreateCommand(IAttackable argument) => new AttackCommand(argument);
}
