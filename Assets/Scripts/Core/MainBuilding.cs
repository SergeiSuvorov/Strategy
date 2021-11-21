using Abstractions;
using UnityEngine;
using Tools;

namespace Core
{
    public sealed class MainBuilding : SelectablePresenter, IUnitProducer
    {

        [SerializeField] private GameObject _unitPrefab;
        [SerializeField] private Transform _unitsParent;

        public void ProduceUnit()
        {
            Instantiate(_unitPrefab,
                new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)),
                Quaternion.identity,
                _unitsParent);
        }

       
    }
}