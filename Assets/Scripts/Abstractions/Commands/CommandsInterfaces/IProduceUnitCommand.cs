using UnityEngine;

namespace Abstractions.Commands.CommandsInterfaces
{
    public interface IProduceUnitCommand :  ICommand, IIconHolder
    {
        float ProductionTime { get; }
        int ProductionCost { get; }
        GameObject UnitPrefab { get; }
        string UnitName { get; }
    }

}