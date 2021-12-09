using UnityEngine;
using UserControlSystem;

public class SetRendezvousPointCommandCreator : CancellableCommandCreatorBase<ISetRendezvousPointCommand, Vector3>
{
    protected override ISetRendezvousPointCommand CreateCommand(Vector3 argument) => new SetRendezvousPointCommand(argument);
    
}