using System.Linq;
using Abstractions;
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
    
    private void Start() => _groundPlane = new Plane(_groundTransform.up, 0);

    private void Update()
    {
        if (!Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1))
        {
            return;
        }
        if (_eventSystem.IsPointerOverGameObject())
        {
            return;
        }


        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);
        if (hits.Length == 0)
        {
            return;
        }
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
            if (attackObject !=null)
            {
                Debug.Log("Attack");
                _attackObject.SetValue(attackObject);
            }
            else if (_groundPlane.Raycast(ray, out var enter))
            {
                _groundClicksRMB.SetValue(ray.origin + ray.direction * enter);
            }
        }

    }
}