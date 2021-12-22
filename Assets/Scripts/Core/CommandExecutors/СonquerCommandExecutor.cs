using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Zenject;

namespace Core.CommandExecutors
{
    public class СonquerCommandExecutor : CommandExecutorBase<IConquerCommand>
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private StopCommandExecutor _stopCommandExecutor;

        [Inject] private NavMeshAgent _agent;
        [Inject] private IHealthHolder _ourHealth;
        [Inject(Id = "AttackDistance")] private float _attackingDistance;
        [Inject(Id = "AttackPeriod")] private int _attackingPeriod;
        [Inject(Id = "СonquerPeriod")] private int _сonquerPeriod;



        private Vector3 _ourPosition;
        private Vector3 _targetPosition;
        private Quaternion _ourRotation;

        private int _attackHash = Animator.StringToHash("Attack");
        private int _walkHash = Animator.StringToHash("Walk");
        private int _idleHash = Animator.StringToHash("Idle");

        private readonly Subject<Vector3> _targetPositions = new Subject<Vector3>();
        private readonly Subject<Quaternion> _targetRotations = new Subject<Quaternion>();
        private readonly Subject<IConquerable> _conquerTargets = new Subject<IConquerable>();


        private Transform _targetTransform;
        private AttackOperation _currentAttackOp;

        [Inject]
        protected void Init()
        {

            _conquerTargets
                .ObserveOnMainThread()
                .Subscribe(StartСonquerTargets);

            _targetPositions
                .Select(value => new Vector3((float)Math.Round(value.x, 2), (float)Math.Round(value.y, 2), (float)Math.Round(value.z, 2)))
                .Distinct()
                .ObserveOnMainThread()
                .Subscribe(StartMovingToPosition);

            _targetRotations
                .ObserveOnMainThread()
                .Subscribe(SetСonquerRotation);
        }

        private void SetСonquerRotation(Quaternion targetRotation)
        {
            transform.rotation = targetRotation;
        }
        private void StartСonquerTargets(IConquerable target)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            _animator.SetTrigger(_attackHash);

            var targetFactionMember = (target as Component).GetComponent<FactionMember>();
            var factionMember = GetComponent<FactionMember>();

            targetFactionMember.SetFaction(factionMember.FactionId);

            _currentAttackOp.Cancel();
            Destroy(gameObject);
        }

        public override async Task ExecuteSpecificCommand(IConquerCommand command)
        {
            await ExecuteAttackCommand(command);
        }

        private void StartAttackingTargets(IAttackable target)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            _animator.SetTrigger(_attackHash);
            target.ReceiveDamage(GetComponent<IDamageDealer>().Damage);
        }

        private void StartMovingToPosition(Vector3 position)
        {
            _agent.destination = position;
            _animator.SetTrigger(_walkHash);
        }

        public async Task ExecuteAttackCommand(IConquerCommand command)
        {
            _targetTransform = (command.СonquerTarget as Component).transform;
            _currentAttackOp = new AttackOperation(this, command.СonquerTarget);
            Update();
            _stopCommandExecutor.CancellationToken = new CancellationTokenSource();
            try
            {
                await _currentAttackOp.WithCancellation(_stopCommandExecutor.CancellationToken.Token);
            }
            catch
            {
                _currentAttackOp.Cancel();
            }
            _animator.SetTrigger(_idleHash);
            _currentAttackOp = null;
            _targetTransform = null;
            _stopCommandExecutor.CancellationToken = null;
        }

        protected void Update()
        {
            if (_currentAttackOp == null)
            {
                return;
            }

            lock (this)
            {
                _ourPosition = transform.position;
                _ourRotation = transform.rotation;
                if (_targetTransform != null)
                {
                    _targetPosition = _targetTransform.position;
                }
            }
        }

        #region AttackOperation

        public sealed class AttackOperation : IAwaitable<AsyncExtensions.Void>
        {
            public class AttackOperationAwaiter : AwaiterBase<AsyncExtensions.Void>
            {
                private AttackOperation _attackOperation;

                public AttackOperationAwaiter(AttackOperation attackOperation)
                {
                    _attackOperation = attackOperation;
                    attackOperation.OnComplete += ONComplete;
                }

                private void ONComplete()
                {
                    _attackOperation.OnComplete -= ONComplete;
                    SetResult(new AsyncExtensions.Void());
                    CompletedAction();
                }
            }

            private event Action OnComplete;

            private readonly СonquerCommandExecutor _attackCommandExecutor;
            private readonly IConquerable _target;
            private readonly FactionMember _targetFactionMember;
            private readonly FactionMember _factionMember;

            private bool _isCancelled;
            public AttackOperation(СonquerCommandExecutor attackCommandExecutor, IConquerable target)
            {
                _factionMember = (attackCommandExecutor as Component).GetComponent<FactionMember>();
                _targetFactionMember = (target as Component).GetComponent<FactionMember>();
               
                _target = target;


                _attackCommandExecutor = attackCommandExecutor;

                var thread = new Thread(AttackAlgorythm);
                thread.Start();
            }

            public void Cancel()
            {
                _isCancelled = true;
                OnComplete?.Invoke();
            }

            private void AttackAlgorythm(object obj)
            {
                while (true)
                {
                    if (
                        _attackCommandExecutor == null
                        || _attackCommandExecutor._ourHealth.Health == 0
                        || _target.Health == 0
                        || _factionMember.FactionId == _targetFactionMember.FactionId
                        || _isCancelled
                        )
                    {
                        OnComplete?.Invoke();
                        return;
                    }

                    var targetPosition = default(Vector3);
                    var ourPosition = default(Vector3);
                    var ourRotation = default(Quaternion);
                    lock (_attackCommandExecutor)
                    {
                        targetPosition = _attackCommandExecutor._targetPosition;
                        ourPosition = _attackCommandExecutor._ourPosition;
                        ourRotation = _attackCommandExecutor._ourRotation;
                    }

                    var vector = targetPosition - ourPosition;
                    var distanceToTarget = vector.magnitude;
                    if (distanceToTarget > _attackCommandExecutor._attackingDistance)
                    {
                        var finalDestination = targetPosition - vector.normalized * (_attackCommandExecutor._attackingDistance * 0.9f);
                        _attackCommandExecutor
                    ._targetPositions.OnNext(finalDestination);
                        Thread.Sleep(100);
                    }
                    else if (ourRotation != Quaternion.LookRotation(vector))
                    {
                        _attackCommandExecutor.
                    _targetRotations
                    .OnNext(Quaternion.LookRotation(vector));
                    }
                    else
                    {
                        _attackCommandExecutor._conquerTargets.OnNext(_target);
                        Thread.Sleep(_attackCommandExecutor._attackingPeriod);
                    }
                }
            }

            public IAwaiter<AsyncExtensions.Void> GetAwaiter()
            {
                return new AttackOperationAwaiter(this);
            }
        }

        #endregion
    }
}
