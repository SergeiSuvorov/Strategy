using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;

public class PatrolCommand : IPatrolCommand
{
    private readonly Vector3 _patrolPoint;
    public Vector3 PatrolPoint => _patrolPoint;
    public PatrolCommand (Vector3 patrolPoint)
    {
        _patrolPoint = patrolPoint;
    }
}
