using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using Utils;
using Zenject;

namespace UserControlSystem.CommandsRealization
{
    public class ProduceUnitModCommand : IProduceUnitCommand
    {
        public GameObject UnitPrefab => _unitPrefab;
        [Inject(Id = "Chomper Mod")] public string UnitName { get; }
        [Inject(Id = "Chomper Mod")] public Sprite Icon { get; }
        [Inject(Id = "Chomper Mod")] public float ProductionTime { get; }
        [Inject(Id = "Chomper Mod")] public int ProductionCost { get; }

        [InjectAsset("Chomper Mod")] private GameObject _unitPrefab;

    }
}