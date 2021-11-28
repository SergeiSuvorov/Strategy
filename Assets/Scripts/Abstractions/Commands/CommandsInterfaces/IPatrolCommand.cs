using UnityEngine;

namespace Abstractions.Commands.CommandsInterfaces
{
    public interface IPatrolCommand : ICommand
    {
        Vector3 PatrolPoint { get; }
    }
}