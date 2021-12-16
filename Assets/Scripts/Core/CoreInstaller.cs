using UnityEngine;
using Zenject;

namespace Core
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private GameStatus _gameStatus; 
        [SerializeField] private EconomicModule _economicModule;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TimeModel>().AsSingle();
            Container.Bind<IGameStatus>().FromInstance(_gameStatus);
            Container.Bind<IShowFactionMoney>().FromInstance(_economicModule);
        }
    }
}