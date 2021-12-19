using Abstractions;
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
    [SerializeField] private Sprite _chomperSprite;
    [SerializeField] private Sprite _chomperModSprite;
    [SerializeField] private Sprite _spitteSprite;
    public override void InstallBindings()
    {
        Container.BindInstances(_assetsContext, _groundClicks, _attackable, _selectables);

        Container.Bind<IAwaitable<IAttackable>>()
            .FromInstance(_attackable);
        Container.Bind<IAwaitable<Vector3>>()
             .FromInstance(_groundClicks);
        Container.Bind<IAwaitable<ISelectable>>()
            .FromInstance(_selectables);


        Container.Bind<Sprite>().WithId("Chomper").FromInstance(_chomperSprite);
        Container.Bind<Sprite>().WithId("Chomper Mod").FromInstance(_chomperModSprite);
        Container.Bind<Sprite>().WithId("Spitter").FromInstance(_spitteSprite);
    }

}