using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlane : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _mouseObject;
    [SerializeField] private float _planeHeight;

    public Transform MouseTransform
    {
        get { return _mouse.transform; }
    }

    private GameObject _mouse;
    private Plane _plane;

    private void Start()
    {
        InitializeMouseObject();

        _plane = new Plane(Vector3.up, _player.position + new Vector3(0f, _planeHeight, 0f));
    }

    private void InitializeMouseObject()
    {
        _mouse = Instantiate(_mouseObject);

        var renderer = _mouse.GetComponentInChildren<Renderer>();

        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
        Cursor.visible = false;
    }

    private void Update()
    {
        _plane.SetNormalAndPosition(Vector3.up, _player.position + new Vector3(0f, _planeHeight, 0f));

        FollowMouse();
    }

    private void FollowMouse()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(_plane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);

            _mouse.transform.position = point;
        }

        Rotation();
    }

    private void Rotation()
    {
        Vector3 directionToPlayer = (_player.position - _mouse.transform.position).normalized;

        if (Vector3.Distance(_player.position, _mouse.transform.position) > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer);

            _mouse.transform.rotation = Quaternion.Euler(90f, targetRotation.eulerAngles.y, 0f);
        }
    }
}
