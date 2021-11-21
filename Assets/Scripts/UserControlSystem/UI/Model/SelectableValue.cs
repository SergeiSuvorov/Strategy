using System;
using Abstractions;
using UnityEngine;


namespace UserControlSystem
{
    [CreateAssetMenu(fileName = nameof(SelectableValue), menuName = "Strategy Game/" + nameof(SelectableValue), order = 0)]
    public class SelectableValue : ScriptableObject
    {
        public ISelectable CurrentValue { get; private set; }
        public event Action<ISelectable> OnSelected;

        public void SetValue(ISelectable value)
        {
            if (CurrentValue != null)
                CurrentValue.OnDeselected();

            CurrentValue = value;
            OnSelected?.Invoke(value);

            if(CurrentValue!=null)
                CurrentValue.OnSelected();
        }
    }
}