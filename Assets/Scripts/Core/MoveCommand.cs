using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;

namespace Core
{
    public class MoveCommand : CommandExecutorBase<IMoveCommand>
    {
        [SerializeField] private float _speed;
        public float Speed => _speed;
        public override void ExecuteSpecificCommand(IMoveCommand command)
        {
            Debug.Log("Moving with speed " + _speed );
        }
    }
}