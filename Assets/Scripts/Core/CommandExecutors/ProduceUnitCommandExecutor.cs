using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UserControlSystem.CommandsRealization;
using Zenject;
using Random = UnityEngine.Random;

namespace Core.CommandExecutors
{
    public class ProduceUnitCommandExecutor : CommandExecutorBase<IProduceUnitCommand>, IUnitProducer
    {
        public IReadOnlyReactiveCollection<IUnitProductionTask> Queue => _queue;

        [SerializeField] private Transform _unitsParent;
        [SerializeField] private int _maximumUnitsInQueue = 6;
        [Inject] private DiContainer _diContainer;

        private MainBuilding _mainBuilding;
        private int _factionId;

        private ReactiveCollection<IUnitProductionTask> _queue = new ReactiveCollection<IUnitProductionTask>();
        private void Awake()
        {
            _mainBuilding = GetComponent<MainBuilding>();
            _factionId = GetComponent<FactionMember>().FactionId;
        }

        private void Update()
        {
            if (_queue.Count == 0)
            {
                return;
            }

            var innerTask = (UnitProductionTask)_queue[0];
            innerTask.TimeLeft -= Time.deltaTime;
            if (innerTask.TimeLeft <= 0)
            {
                RemoveTaskAtIndex(0);

                var unit = _diContainer.InstantiatePrefab(innerTask.UnitPrefab, transform.position, Quaternion.identity, _unitsParent);
                var unitMover = unit.GetComponent<CommandExecutorBase<IMoveCommand>>();
                var factionMember = unit.GetComponent<FactionMember>();
                factionMember.SetFaction(_factionId);
                unitMover.ExecuteSpecificCommand(new MoveCommand(_mainBuilding.RendezvousPoint));
            }
        }

        public void Cancel(int index) => RemoveTaskAtIndex(index);

        private void RemoveTaskAtIndex(int index)
        {
            for (int i = index; i < _queue.Count - 1; i++)
            {
                _queue[i] = _queue[i + 1];
            }
            _queue.RemoveAt(_queue.Count - 1);
        }

        public override async Task ExecuteSpecificCommand(IProduceUnitCommand command)
        {

            if (EconomicModule.GetFactionMoneyCount(_factionId) < command.ProductionCost)
                Debug.Log("Не хватает денег");
            else if (_queue.Count >= _maximumUnitsInQueue)
                Debug.Log("Очередь производства заполнена");
            else
            {
                _queue.Add(new UnitProductionTask(command.ProductionTime, command.Icon, command.UnitPrefab, command.UnitName));
                EconomicModule.ChangeMoneyCount(_factionId, -command.ProductionCost);
            }
        }
    }
}