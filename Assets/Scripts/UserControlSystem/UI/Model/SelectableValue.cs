using System;
using Abstractions;
using UnityEngine;

namespace UserControlSystem
{
    [CreateAssetMenu(fileName = nameof(SelectableValue), menuName = "Strategy Game/" + nameof(SelectableValue), order = 0)]
    public class SelectableValue : ScriptableObjectValue<ISelectable>
    {

        public override void SetValue(ISelectable value)
        {
            if (CurrentValue != null)
                CurrentValue.OnDeselected();

            base.SetValue(value);

            if (CurrentValue != null)
                CurrentValue.OnSelected();
        }
    }
}