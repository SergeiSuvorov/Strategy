using UnityEngine;
using UserControlSystem;
using Utils;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableObjectsInstaller", menuName = "Installers/ScriptableObjectsInstaller")]
public class ScriptableObjectsInstaller : ScriptableObjectInstaller<ScriptableObjectsInstaller>
{
    [SerializeField] private AssetsContext _assetsContext;
    [SerializeField] private Vector3Value _groundClicks;
    [SerializeField] private AttackableValue _attackable;
    [SerializeField] private SelectableValue _selectables;

    public override void InstallBindings()
    {
        Container.BindInstances(_assetsContext, _groundClicks, _attackable, _selectables);
    }

}