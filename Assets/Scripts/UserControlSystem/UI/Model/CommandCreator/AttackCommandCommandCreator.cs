using Abstractions;
using Abstractions.Commands.CommandsInterfaces;
using System;
using UnityEngine;
using UserControlSystem;
using Utils;
using Zenject;

public class AttackCommandCommandCreator : CommandCreatorBase<IAttackCommand>
{
    [Inject] private AssetsContext _context;

    private Action<IAttackCommand> _creationCallback;

    [Inject]
    private void Init(AttackableValue attackableValue)
    {
        Debug.Log("Attack Init");
        attackableValue.OnValueChange += SetAttackableObject;
    }

    private void SetAttackableObject(IAttackable attackClick)
    {
        _creationCallback?.Invoke(_context.Inject(new AttackCommand(attackClick)));
        _creationCallback = null;
    }

    public override void ProcessCancel()
    {
        base.ProcessCancel();
        _creationCallback = null;
    }

    protected override void ClassSpecificCommandCreation(Action<IAttackCommand> creationCallback)
    {
        _creationCallback = creationCallback;
    }
}


