using UniRx;
using UnityEngine;
using Zenject;

namespace Core
{
    public class AutoAttackAgent : MonoBehaviour
    {
      [Inject] private ICommandsQueue _queue;

        private void Start()
        {
            AutoAttackEvaluator.AutoAttackCommands
                .ObserveOnMainThread()
                .Where(command => command.Attacker == gameObject)
                .Where(command => command.Attacker != null && command.Target != null)
                .Subscribe(command => AutoAttack(command.Target))
                .AddTo(this);
        }

        private void AutoAttack(GameObject target)
        {
            _queue.Clear();
            _queue.EnqueueCommand(new AutoAttackCommand(target.GetComponent<IAttackable>()));
        }
    }
}