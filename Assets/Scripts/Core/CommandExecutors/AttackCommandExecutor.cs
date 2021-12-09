using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using System.Threading.Tasks;

namespace Core
{
    public class AttackCommandExecutor: CommandExecutorBase<IAttackCommand>
    {
        [SerializeField] private float _damage;
        public float Damage => _damage;

        public override async Task ExecuteSpecificCommand(IAttackCommand command)
        {
            Debug.Log("Attack. Damage - "+_damage);
        }
    }
}