using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using UserControlSystem;
using Utils;
using Zenject;

public class AIInstaller : MonoInstaller
{
    [SerializeField] private AssetsContext _legacyContext;
    [SerializeField] private int _fractionID;
    [SerializeField] private int _maxUnitCoiunt;
    public override void InstallBindings()
    {
        Container.Bind<CommandCreatorBase<IProduceUnitCommand>>()
               .To<ProduceUnitCommandCommandCreator>().AsTransient();
        Container.Bind<CancellableCommandCreatorBase<IMoveCommand, Vector3>>()
            .To<MoveCommandCommandCreator>().AsTransient();
        Container.Bind<CommandCreatorBase<IAttackCommand>>()
           .To<AttackCommandCommandCreator>().AsTransient();
        Container.Bind<CommandCreatorBase<IStopCommand>>()
         .To<StopCommandCommandCreator>().AsTransient();
        Container.Bind<CommandCreatorBase<ISetRendezvousPointCommand>>()
         .To<SetRendezvousPointCommandCreator>().AsTransient();

        Container.Bind<AIEconomic> ()
         .To<AIEconomic>().AsTransient();
        Container.Bind<AIAttackModel>()
        .To<AIAttackModel>().AsTransient();
        Container.Bind<AIUnitManager>()
        .To<AIUnitManager>().AsTransient();
        Container.Bind<AIUnitProduce>()
        .To<AIUnitProduce>().AsTransient();
        

        Container.Bind<float>().WithId("Chomper").FromInstance(5f);
        Container.Bind<string>().WithId("Chomper").FromInstance("Chomper");
        Container.Bind<int>().WithId("Chomper").FromInstance(100);

        Container.Bind<float>().WithId("Chomper Mod").FromInstance(5f);
        Container.Bind<string>().WithId("Chomper Mod").FromInstance("Chomper Mod");
        Container.Bind<int>().WithId("Chomper Mod").FromInstance(200);

        Container.Bind<float>().WithId("Spitter").FromInstance(5f);
        Container.Bind<string>().WithId("Spitter").FromInstance("Spitter");
        Container.Bind<int>().WithId("Spitter").FromInstance(400);

        Container.Bind<int>().WithId("Fraction ID").FromInstance(_fractionID);
        Container.Bind<int>().WithId("Max Unit Count").FromInstance(_maxUnitCoiunt);

    }
}