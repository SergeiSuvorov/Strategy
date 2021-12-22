using UnityEngine;

namespace UserControlSystem
{
    [CreateAssetMenu(fileName = nameof(СonquerableValue), menuName = "Strategy Game/" + nameof(СonquerableValue), order = 0)]
    public class СonquerableValue : StatelessScriptableObjectValueBase<IConquerable>
    {

    }
}