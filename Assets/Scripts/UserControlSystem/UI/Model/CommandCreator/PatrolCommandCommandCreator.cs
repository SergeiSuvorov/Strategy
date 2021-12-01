using Abstractions.Commands.CommandsInterfaces;
using System;
using UnityEngine;
using UserControlSystem;
using UserControlSystem.CommandsRealization;
using Utils;
using Zenject;

public class PatrolCommandCommandCreator : CancellableCommandCreatorBase<IPatrolCommand, Vector3>
{
    [Inject] private SelectableValue _selectable;

    protected override IPatrolCommand CreateCommand(Vector3 argument) => new PatrolCommand(_selectable.CurrentValue.PivotPoint.position, argument);
}



