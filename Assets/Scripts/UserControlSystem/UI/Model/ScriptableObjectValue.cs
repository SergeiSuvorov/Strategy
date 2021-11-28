using System;
using UnityEngine;

namespace UserControlSystem
{
    public abstract class ScriptableObjectValue<T> : ScriptableObject
    {
        public T CurrentValue { get; private set; }
        public Action<T> OnValueChange;

        public virtual void SetValue(T value)
        {
            CurrentValue = value;
            OnValueChange?.Invoke(value);
        }
    }

}