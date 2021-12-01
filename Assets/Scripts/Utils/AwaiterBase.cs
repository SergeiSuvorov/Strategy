using System;
using UnityEngine;

namespace Utils
{
    public abstract class AwaiterBase<T> : IAwaiter<T>
    {
        private Action _continuation;
        private bool _isCompleted;
        private T _result;
        public bool IsCompleted =>  _isCompleted;

        public T GetResult()
        {
            return _result;
        }

        protected virtual void SetResult(T result)
        {
            _result = result;
        }

        protected void CompletedAction()
        {
            _isCompleted = true;
            _continuation?.Invoke();
        }
        public void OnCompleted(Action continuation)
        {
            if (_isCompleted)
            {
                continuation?.Invoke();
            }
            else
            {
                _continuation = continuation;
            }
        }
    }
}