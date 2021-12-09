using Abstractions;
using UnityEngine;
using Tools;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;

namespace Core
{
    public class Chomper : MonoBehaviour, ISelectable, IUnit
    {
        public float Health => _health;
        public float MaxHealth => _maxHealth;
        public Sprite Icon => _icon;
        public Transform PivotPoint => _pivotPoint;

        [SerializeField] private float _maxHealth = 100;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Outline _selectedOutline;
        [SerializeField] private Transform _pivotPoint;

        private float _health = 100;
        public void OnSelected()
        {
            _selectedOutline.enabled = true;
        }

        public void OnDeselected()
        {
            _selectedOutline.enabled = false;
        }
    }
}