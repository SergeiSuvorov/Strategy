using Abstractions.Commands.CommandsInterfaces;
using System;
using UnityEngine;
using UserControlSystem;
using Utils;
using Zenject;

public class PatrolCommandCommandCreator : CommandCreatorBase<IPatrolCommand>
{
    [Inject] private AssetsContext _context;

    private Action<IPatrolCommand> _creationCallback;

    [Inject]
    private void Init(Vector3Value groundClick)
    {
        Debug.Log("Patrol Init");
        groundClick.OnValueChange += SetPatrolPoint;
    }

    private void SetPatrolPoint(Vector3 groundClick)
    {
        _creationCallback?.Invoke(_context.Inject(new PatrolCommand(groundClick)));
        _creationCallback = null;
    }


    public override void ProcessCancel()
    {
        base.ProcessCancel();

        _creationCallback = null;
    }

    protected override void ClassSpecificCommandCreation(Action<IPatrolCommand> creationCallback)
    {
        _creationCallback = creationCallback;
    }
}


