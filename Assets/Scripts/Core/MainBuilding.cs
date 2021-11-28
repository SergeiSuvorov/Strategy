using Abstractions;
using UnityEngine;
using Tools;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using System.Collections.Generic;

namespace Core
{
   
    public class MainBuilding : CommandExecutorBase<IProduceUnitCommand>, ISelectable, IAttackable
    {
        public float Health => _health;
        public float MaxHealth => _maxHealth;
        public Sprite Icon => _icon;

        [SerializeField] private float _maxHealth = 1000;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Outline _selectedOutline;
        [SerializeField] private Transform _unitsParent;

        private float _health = 1000;

        public void OnSelected()
        {
            _selectedOutline.enabled = true;
        }

        public void OnDeselected()
        {
            _selectedOutline.enabled = false;
        }

        public override void ExecuteSpecificCommand(IProduceUnitCommand command)
        {
            Instantiate(command.UnitPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity, _unitsParent);
        }
    }
}