using System.Linq;
using Abstractions;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UserControlSystem;
using Zenject;

public sealed class MouseInteractionPresenter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [Inject] private SelectableValue _selectedObject;
    [Inject] private AttackableValue _attackObject;
    [SerializeField] private EventSystem _eventSystem;

    [Inject] private Vector3Value _groundClicksRMB;
    [SerializeField] private Transform _groundTransform;

    private Plane _groundPlane;

    [Inject]
    private void Init()
    {
        _groundPlane = new Plane(_groundTransform.up, 0);

        var lBMClickStreem = Observable.EveryUpdate()
            .Where(_ => !_eventSystem.IsPointerOverGameObject())
            .Where(_ => Input.GetMouseButton(0));
        var rBMClickStreem = Observable.EveryUpdate()
            .Where(_ => !_eventSystem.IsPointerOverGameObject())
            .Where(_ => Input.GetMouseButton(1));

        var lBMClickRay = lBMClickStreem
            .Select(_ => _camera.ScreenPointToRay(Input.mousePosition));
        var rBMClickRay = rBMClickStreem
            .Select(_ => _camera.ScreenPointToRay(Input.mousePosition));

        var lBMClickRayHits = lBMClickRay
            .Select(ray => Physics.RaycastAll(ray));
        var rBMClickRayHits = rBMClickRay
            .Select(ray => (ray, Physics.RaycastAll(ray)));

        lBMClickRayHits.Subscribe(hits =>
        {
            if (GetHitResult<ISelectable>(hits, out var selectable))
            {
                _selectedObject.SetValue(selectable);
            }
        });

        rBMClickRayHits.Subscribe(data =>
        {
            var (ray, hits) = data;
            if (GetHitResult<IAttackable>(hits, out var attackable))
            {
                _attackObject.SetValue(attackable);
            }
            else if (_groundPlane.Raycast(ray, out var enter))
            {
                _groundClicksRMB.SetValue(ray.origin + ray.direction * enter);
            }
        });

    }


    private void Initialized()
    {

        var notUiClickStreem = Observable.EveryUpdate()
            .Where(_ => !_eventSystem.IsPointerOverGameObject());

        var ClickStreem = notUiClickStreem.Where(_ => (Input.GetMouseButton(0) || Input.GetMouseButton(1)));

        var ClickRay = ClickStreem
            .Select(_ => _camera.ScreenPointToRay(Input.mousePosition));

        var ClickRayHits = ClickRay
            .Select(ray => (ray, Physics.RaycastAll(ray)));

        ClickRayHits.Subscribe(data =>
        {
            var (ray, hits) = data;

            var clickObject = hits
            .Select(hit => hit.collider.GetComponentInParent<ISelectable>())
            .Where(c => c != null)
            .FirstOrDefault();

            if (Input.GetMouseButtonUp(0))
            {
                _selectedObject.SetValue(clickObject);
            }
            else
            {
                var attackObject = clickObject as IAttackable;
                if (attackObject != null)
                {
                    Debug.Log("Attack");
                    _attackObject.SetValue(attackObject);
                }
                else if (_groundPlane.Raycast(ray, out var enter))
                {
                    _groundClicksRMB.SetValue(ray.origin + ray.direction * enter);
                }
            }
        });
    }

    private bool GetHitResult<T>(RaycastHit[] hits, out T result) where T : class
    {
        result = default;
        if (hits.Length == 0)
        {
            return false;
        }
        result = hits
            .Select(hit => hit.collider.GetComponentInParent<T>())
            .Where(c => c != null)
            .FirstOrDefault();
        return result != default;
    }

}