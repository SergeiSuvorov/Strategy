using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;

namespace Core
{
    public class AttackCommand: CommandExecutorBase<IAttackCommand>
    {
        [SerializeField] private float _damage;
        public float Damage => _damage;

        public override void ExecuteSpecificCommand(IAttackCommand command)
        {
            Debug.Log("Attack. Damage - "+_damage);
        }
    }
}