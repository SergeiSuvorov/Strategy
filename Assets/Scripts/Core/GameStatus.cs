using System;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Core
{
    public class GameStatus : MonoBehaviour, IGameStatus
    {
        public IObservable<int> Status => _status;
        private Subject<int> _status = new Subject<int>();

        private void CheckStatus(object state)
        {
            //Debug.Log(FactionMember.FactionsCount);
            if (FactionMember.FactionsCount == 0)
            {
                _status.OnNext(0);
            }
            else if (FactionMember.FactionsCount == 1)
            {
                _status.OnNext(FactionMember.GetWinner());
            }
        }

        private void Update()
        {
            ThreadPool.QueueUserWorkItem(CheckStatus);
        }
    }
}

