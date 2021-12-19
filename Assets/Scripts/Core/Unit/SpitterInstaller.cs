using Abstractions;
using Core;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Abstractions
{
    public class SpitterInstaller : MonoInstaller
    {
        [SerializeField] private AttackerParallelnfoUpdater _attackerParallelnfoUpdater;
        [SerializeField] private FactionMemberParallelInfoUpdater _factionMemberParallelInfoUpdater;
        [SerializeField] private int _conquerPeriod;
        [SerializeField] private int _attackPeriod;
        [SerializeField] private float _attackDistance;
    

        public override void InstallBindings()
        {
            Container.Bind<IHealthHolder>().FromComponentInChildren();
            Container.Bind<float>().WithId("AttackDistance").FromInstance(_attackDistance);
            Container.Bind<int>().WithId("СonquerPeriod").FromInstance(_conquerPeriod);
            Container.Bind<int>().WithId("AttackPeriod").FromInstance(_attackPeriod);

            Container.Bind<IAutomaticAttacker>().FromComponentInChildren();
            Container
            .Bind<ITickable>()
            .FromInstance(_attackerParallelnfoUpdater);
            Container
                .Bind<ITickable>()
                .FromInstance(_factionMemberParallelInfoUpdater);
            Container.Bind<IFactionMember>().FromComponentInChildren();
            Container.Bind<ICommandsQueue>().FromComponentInChildren();
        }
    }
}