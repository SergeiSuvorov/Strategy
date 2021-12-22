using UnityEngine;

public class SetRendezvousPointCommand : ISetRendezvousPointCommand
{

    public Vector3 RendezvousPoint { get; }

    public SetRendezvousPointCommand(Vector3 rendezvousPoint)
    {
        Debug.Log("Set Rendezvous Point " + rendezvousPoint);
        RendezvousPoint = rendezvousPoint;
    }
}
