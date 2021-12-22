using Abstractions;
using Core;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Assets.Scripts.Abstractions
{
    public class ChomperInstaller : MonoInstaller
    {
        [SerializeField] private AttackerParallelnfoUpdater _attackerParallelnfoUpdater;
        [SerializeField] private FactionMemberParallelInfoUpdater _factionMemberParallelInfoUpdater;
        public override void InstallBindings()
        {
            Container.Bind<IHealthHolder>().FromComponentInChildren();
            Container.Bind<float>().WithId("AttackDistance").FromInstance(5f);
            Container.Bind<int>().WithId("AttackPeriod").FromInstance(1400);
            Container.Bind<int>().WithId("Damage").FromInstance(GetComponent<IDamageDealer>().Damage);

            Container.Bind<IAutomaticAttacker>().FromComponentInChildren();
            Container
            .Bind<ITickable>()
            .FromInstance(_attackerParallelnfoUpdater);
            Container
                .Bind<ITickable>()
                .FromInstance(_factionMemberParallelInfoUpdater);
            Container.Bind<IFactionMember>().FromComponentInChildren();
            Container.Bind<ICommandsQueue>().FromComponentInChildren();
            Container.Bind<NavMeshAgent>().FromComponentInChildren();
        }

    }
}