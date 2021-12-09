using Abstractions.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetRendezvousPointCommand : ICommand
{
    Vector3 RendezvousPoint { get; }
}

