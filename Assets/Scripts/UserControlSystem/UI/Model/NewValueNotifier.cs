using System;
using UnityEngine;
using Utils;

namespace UserControlSystem
{
    public class NewValueNotifier<TAwaited> : AwaiterBase<TAwaited>
    {
        private readonly ScriptableObjectValue<TAwaited> _scriptableObjectValueBase;


        public NewValueNotifier(ScriptableObjectValue<TAwaited> scriptableObjectValueBase)
        {
            _scriptableObjectValueBase = scriptableObjectValueBase;
            _scriptableObjectValueBase.OnNewValue += ONNewValue;
        }

        private void ONNewValue(TAwaited obj)
        {
            Debug.Log(obj.ToString());
            _scriptableObjectValueBase.OnNewValue -= ONNewValue;
            SetResult(obj);
            CompletedAction();
        }

    }
}