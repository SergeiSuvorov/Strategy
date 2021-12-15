using Assets.Scripts.Abstractions;
using Core;
using UnityEngine;
using Zenject;

public class MainBuildingInstaller : MonoInstaller
{
    [SerializeField] private FactionMemberParallelInfoUpdater _factionMemberParallelInfoUpdater;

    public override void InstallBindings()
    {
        Container
        .Bind<ITickable>()
        .FromInstance(_factionMemberParallelInfoUpdater);
        Container.Bind<IFactionMember>().FromComponentInChildren();
    }
}


