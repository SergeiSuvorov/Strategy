using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using Utils;
using Zenject;

namespace UserControlSystem.CommandsRealization
{
    public class ProduceSpitterCommand : IProduceUnitCommand
    {
        public GameObject UnitPrefab => _unitPrefab;
        [Inject(Id = "Spitter")] public string UnitName { get; }
        [Inject(Id = "Spitter")] public Sprite Icon { get; }
        [Inject(Id = "Spitter")] public float ProductionTime { get; }
        [Inject(Id = "Spitter")] public int ProductionCost { get; }

        [InjectAsset("Spitter")] private GameObject _unitPrefab;

    }
}