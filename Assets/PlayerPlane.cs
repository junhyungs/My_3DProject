using UnityEngine;
using Cinemachine;

public class PlayerPlane : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _mouseObject;
    [SerializeField] private float _planeHeight;

    public CinemachineVirtualCamera VirtualCamera { get; set; }
    public CinemachineTransposer Transposer { get; set; }
    public Vector3 Point { get; set; }

    private GameObject _mouse;
    private Plane _plane;
   
    private void OnEnable()
    {
        SetActiveMouseObject(true);
    }

    private void OnDisable()
    {
        SetActiveMouseObject(false);
    }

    private void SetActiveMouseObject(bool active)
    {
        if(_mouse != null)
        {
            _mouse.SetActive(active);
        }
    }

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

            Point = point;

            MouseMovement(point);
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

    private void MouseMovement(Vector3 point)
    {
        bool isGetMouseButton_Right = Input.GetMouseButton(1);
        bool isGetKeyDown_LeftShift = Input.GetKey(KeyCode.LeftShift);

        if(isGetMouseButton_Right || isGetKeyDown_LeftShift)
        {
            SetTransposer(2f);

            if (isGetMouseButton_Right && isGetKeyDown_LeftShift)
            {
                SetMouseObjectPosition(point, 20f);
            }
            else if (isGetMouseButton_Right)
            {
                SetMouseObjectPosition(point, 8f);
            }
            else if(isGetKeyDown_LeftShift)
            {
                SetMouseObjectPosition(point, 15f);
            }

            if(VirtualCamera.m_Follow != _mouse.transform)
            {
                VirtualCamera.m_Follow = _mouse.transform;
                VirtualCamera.LookAt = _mouse.transform;
            }
        }
        else
        {
            SetTransposer(0f);

            if(VirtualCamera.m_Follow != _player)
            {
                VirtualCamera.m_Follow = _player;
                VirtualCamera.m_LookAt = _player;
            }

            _mouse.transform.position = point;
        }
    }

    private void SetTransposer(float Damping)
    {
        if(Transposer.m_XDamping != Damping)
        {
            Transposer.m_XDamping = Damping;
            Transposer.m_YDamping = Damping;
            Transposer.m_ZDamping = Damping;
        }
    }

    private void SetMouseObjectPosition(Vector3 point, float maxDistance)
    {
        Vector3 moveDirection = (point - _player.position).normalized;

        float currentDistence = Vector3.Distance(point, _player.position);

        if(currentDistence > maxDistance)
        {
            point = _player.position + moveDirection * maxDistance;
        }

        _mouse.transform.position = point;
    }
}
