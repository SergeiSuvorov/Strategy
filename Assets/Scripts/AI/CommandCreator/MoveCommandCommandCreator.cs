using Abstractions.Commands.CommandsInterfaces;
using System;
using UnityEngine;
using UserControlSystem;
using UserControlSystem.CommandsRealization;
using Utils;
using Zenject;

public class MoveCommandCommandCreator : CancellableCommandCreatorBase<IMoveCommand, Vector3>
{
    protected override IMoveCommand CreateCommand(Vector3 argument) => new MoveCommand(argument);


}


