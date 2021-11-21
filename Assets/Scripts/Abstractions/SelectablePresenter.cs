using Abstractions;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Abstractions
{
    public abstract class SelectablePresenter : MonoBehaviour, ISelectable
    {
        public float Health => _health;
        public float MaxHealth => _maxHealth;
        public Sprite Icon => _icon;

        [SerializeField] private float _maxHealth = 1000;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Outline _selectedOutline;

        private float _health = 1000;

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
