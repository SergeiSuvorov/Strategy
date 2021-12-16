using Abstractions;
using UnityEngine;
using Tools;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{

    public class MainBuilding : MonoBehaviour, ISelectable, IAttackable, IUnitTypeCreater
    {
        public float Health => _health;
        public float MaxHealth => _maxHealth;
        public Sprite Icon => _icon;
        public Vector3 RendezvousPoint { get; set; }

        public Transform PivotPoint => _pivotPoint;

        public UnitType UnitType => _unitType;

        [SerializeField] private float _maxHealth = 1000;
        [SerializeField] private int createPrefabDelayTime = 5000;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Outline _selectedOutline;
        [SerializeField] private Transform _pivotPoint;
        [SerializeField] private UnitType _unitType;
       private float _health = 1000;
        public void OnSelected()
        {
            _selectedOutline.enabled = true;
        }

        public void OnDeselected()
        {
            _selectedOutline.enabled = false;
        }

        public void ReceiveDamage(int amount)
        {
            if (_health <= 0)
            {
                return;
            }
            _health -= amount;
            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}