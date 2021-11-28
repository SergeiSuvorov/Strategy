using Abstractions.Commands.CommandsInterfaces;
using System;
using UnityEngine;
using UserControlSystem;
using UserControlSystem.CommandsRealization;
using Utils;
using Zenject;

public class MoveCommandCommandCreator : CommandCreatorBase<IMoveCommand>
{
    [Inject] private AssetsContext _context;

    private Action<IMoveCommand> _creationCallback;

    [Inject]
    private void Init(Vector3Value groundClicks)
    {
        Debug.Log("Move Init");
        groundClicks.OnValueChange += onNewValue;
    }

    private void onNewValue(Vector3 groundClick)
    {
        _creationCallback?.Invoke(_context.Inject(new MoveCommand(groundClick)));
        _creationCallback = null;
    }


    public override void ProcessCancel()
    {
        base.ProcessCancel();

        _creationCallback = null;
    }

    protected override void ClassSpecificCommandCreation(Action<IMoveCommand> creationCallback)
    {
        _creationCallback = creationCallback;
    }
}


