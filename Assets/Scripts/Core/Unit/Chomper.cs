using Abstractions;
using UnityEngine;
using Tools;
using UserControlSystem.CommandsRealization;


namespace Core
{
    public class Chomper : MonoBehaviour, ISelectable, IUnit, IDamageDealer,IAttackable, IAutomaticAttacker
    {
        public float Health => _health;
        public float MaxHealth => _maxHealth;
        public Sprite Icon => _icon;
        public Transform PivotPoint => _pivotPoint;
        public float VisionRadius => _visionRadius;
        public int Damage => _damage;

        [SerializeField] private float _maxHealth = 100;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Outline _selectedOutline;
        [SerializeField] private Transform _pivotPoint;
        [SerializeField] private Animator _animator;
        [SerializeField] private StopCommandExecutor _stopCommand;
        [SerializeField] private int _damage = 25;
        [SerializeField] private float _visionRadius = 8f;


        private float _health = 100;
        public void OnSelected()
        {
            if (this == null)
            {
                return;
            }
            _selectedOutline.enabled = true;
        }

        public void OnDeselected()
        {
            if (this == null)
            {
                return;
            }
            _selectedOutline.enabled = false;
        }

       

        private async void Destroy()
        {
            await _stopCommand.ExecuteSpecificCommand(new StopCommand());
            OnDeselected();
            Destroy(gameObject);
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
                _animator.SetTrigger("PlayDead");
                Invoke(nameof(Destroy), 1f);
            }
        }
    }
}