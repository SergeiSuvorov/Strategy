using System;
using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using UserControlSystem.CommandsRealization;
using Utils;
using Zenject;

namespace UserControlSystem
{
    public sealed class ProduceUnitCommandCommandCreator : CommandCreatorBase<IProduceUnitCommand>
    {
        [Inject] private AssetsContext _context;
        [Inject] private DiContainer _diContainer;

        protected override void ClassSpecificCommandCreation(ICommandExecutor commandExecutor, Action<IProduceUnitCommand> creationCallback)
        {
            Debug.Log("ProduceUnitCommandCommandCreator");
            var commandComponent = commandExecutor as Component;
            var unitType= commandComponent.GetComponent<IUnitTypeCreater>().UnitType;
            Debug.Log(unitType);
            var produceUnitCommand = _context.Inject(CreateProduceUnitCommand(unitType));
            _diContainer.Inject(produceUnitCommand);
            creationCallback?.Invoke(produceUnitCommand);
        }

        private IProduceUnitCommand CreateProduceUnitCommand(UnitType unitType)
        {
            switch (unitType)
            {
                case UnitType.Chomper:
                    return new ProduceUnitCommand();
                case UnitType.ChomperMod:
                    return new ProduceUnitModCommand();
                default:
                    return new ProduceUnitCommand();
            }
        }

    }
}