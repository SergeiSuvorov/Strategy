using System;
using Abstractions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core
{
    public sealed class TimeModel : ITimeModel, ITickable
    {
        public IObservable<int> GameTime => _gameTime.Select(f => (int)f);
        public bool IsPaused { get => _isTimePause; set => _isTimePause = value; }

        private readonly ReactiveProperty<float> _gameTime = new ReactiveProperty<float>();
        private bool _isTimePause = false;
        public void Tick()
        {
            if (_isTimePause)
                return;

            _gameTime.Value += Time.deltaTime;
        }

    }
}