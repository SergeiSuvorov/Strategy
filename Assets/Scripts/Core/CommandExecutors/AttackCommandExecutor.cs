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
    public class AttackCommandExecutor : CommandExecutorBase<IAttackCommand>
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected StopCommandExecutor _stopCommandExecutor;

        [Inject] protected IHealthHolder _ourHealth;
        [Inject(Id = "AttackDistance")] protected float _attackingDistance;
        [Inject(Id = "AttackPeriod")] protected int _attackingPeriod;

        protected Vector3 _ourPosition;
        protected Vector3 _targetPosition;
        protected Quaternion _ourRotation;

        protected readonly Subject<Vector3> _targetPositions = new Subject<Vector3>();
        protected readonly Subject<Quaternion> _targetRotations = new Subject<Quaternion>();
        private readonly Subject<IAttackable> _attackTargets = new Subject<IAttackable>();

        protected Transform _targetTransform;
        private AttackOperation _currentAttackOp;
        
        [Inject]
        protected void Init()
        {
            _targetPositions
                .Select(value => new Vector3((float)Math.Round(value.x, 2), (float)Math.Round(value.y, 2), (float)Math.Round(value.z, 2)))
                .Distinct()
                .ObserveOnMainThread()
                .Subscribe(StartMovingToPosition);

            _attackTargets
                .ObserveOnMainThread()
                .Subscribe(StartAttackingTargets);

            _targetRotations
                .ObserveOnMainThread()
                .Subscribe(SetAttackRotation);

            Debug.Log("AttackCommandExecutor Init");
        }
        
        private void SetAttackRotation(Quaternion targetRotation)
        {
            transform.rotation = targetRotation;
        }

        private void StartAttackingTargets(IAttackable target)
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<NavMeshAgent>().ResetPath();
            _animator.SetTrigger(Animator.StringToHash("Attack"));
            target.ReceiveDamage(GetComponent<IDamageDealer>().Damage);
        }

        private void StartMovingToPosition(Vector3 position)
        {
            GetComponent<NavMeshAgent>().destination = position;
            _animator.SetTrigger(Animator.StringToHash("Walk"));
        }
        
        public override async Task ExecuteSpecificCommand(IAttackCommand command)
        {
            _targetTransform = (command.AttackTarget as Component).transform;
            _currentAttackOp = new AttackOperation(this, command.AttackTarget);
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
            _animator.SetTrigger("Idle");
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

            lock(this)
            {
                _ourPosition = transform.position;
                _ourRotation = transform.rotation;
                if (_targetTransform  != null)
                {
                    _targetPosition = _targetTransform .position;
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
	
    		private readonly AttackCommandExecutor _attackCommandExecutor;
    		private readonly IAttackable _target;

    		private bool _isCancelled;
            private readonly FactionMember _targetFactionMember;
            private readonly FactionMember _factionMember;
            public AttackOperation(AttackCommandExecutor attackCommandExecutor, IAttackable target)
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
                			_attackCommandExecutor._attackTargets.OnNext(_target);
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