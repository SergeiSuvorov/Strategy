using Abstractions;
using UnityEngine;
using Tools;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{
   
    public class MainBuilding : CommandExecutorBase<IProduceUnitCommand>, ISelectable, IAttackable
    {
        public float Health => _health;
        public float MaxHealth => _maxHealth;
        public Sprite Icon => _icon;

        public Transform PivotPoint => _pivotPoint;

        [SerializeField] private float _maxHealth = 1000;
        [SerializeField] private int createPrefabDelayTime = 5000;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Outline _selectedOutline;
        [SerializeField] private Transform _pivotPoint;
        [SerializeField] private Transform _unitsParent;

        private float _health = 1000;
        private List<IProduceUnitCommand> _creationQueue = new List<IProduceUnitCommand>();
        public void OnSelected()
        {
            _selectedOutline.enabled = true;
        }

        public void OnDeselected()
        {
            _selectedOutline.enabled = false;
        }

        public override async void ExecuteSpecificCommand(IProduceUnitCommand command)
        {
            _creationQueue.Add(command);
            if (_creationQueue.Count > 1)
                return;
            while (_creationQueue.Count > 0)
            {
                await Task.Delay(createPrefabDelayTime);
                Instantiate(_creationQueue[0].UnitPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity, _unitsParent);
                _creationQueue.Remove(_creationQueue[0]);
                Debug.Log("Осталось произвести - " + _creationQueue.Count);
            }
        }
    }
}