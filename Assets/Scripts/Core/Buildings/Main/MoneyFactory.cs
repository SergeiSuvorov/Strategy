using Abstractions;
using UnityEngine;
using Tools;
using System;

namespace Core
{
    public class MoneyFactory : MonoBehaviour, ISelectable, IConquerable, IGenerateMoney
    {
        public float Health => _health;
        public float MaxHealth => _maxHealth;
        public Sprite Icon => _icon;
        public Vector3 RendezvousPoint { get; set; }

        public Transform PivotPoint => _pivotPoint;

        [SerializeField] private float _maxHealth = 700;
        [SerializeField] private int createPrefabDelayTime = 5000;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Outline _selectedOutline;
        [SerializeField] private Transform _pivotPoint;

        private float _health = 700;

        private float _factoryTime = 5f;
        private float _curentTime = 5f;
        private FactionMember _factoryId;

        private void Start()
        {
            _factoryId= GetComponent<FactionMember>();
        }
        private void Update()
        {
            if(_curentTime>=0)
            {
                _curentTime -= Time.deltaTime;
            }
            else
            {
                _curentTime = _factoryTime;
                EconomicModule.ChangeMoneyCount(_factoryId.FactionId, 100);
            }
        }
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