using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemachineCamera : MonoBehaviour
{
    [Header("PlayerCameraPrefab")]
    [SerializeField] private GameObject m_virtualCameraPrefab;

    [Header("PlayerTargetTransform")]
    [SerializeField] private GameObject m_targetTrans;

    [Header("PlayerLookTransform")]
    [SerializeField] private GameObject m_lookTrans;

    [Header("MaxCameraTransform")]
    [SerializeField] private GameObject m_maxTrans;

    [Header("CameraFollowOffset")]
    [SerializeField] private Vector3 m_CinemachineVirtualCameraFollowOffSet;

    private GameObject m_playerCam;
    private CinemachineVirtualCamera m_virtualCam;
    private CinemachineTransposer _transposer;
    private Vector3 _onDisablePosition;


    private float m_maxDistance = 15f;
    private float _minView = 20f;
    private float _zoomInSpeed = 0.5f;
    private float _currentFieldOfView;

    public GameObject PlayerCam
    {
        get { return m_playerCam; }
        set { m_playerCam = value; }
    }

    private void Awake()
    {
        GameManager.Instance.RegisterDeathAction(DeathCameraZoom);
    }

    private void OnEnable()
    {
        if(m_playerCam != null)
        {
            Vector3 movePosition = transform.position - _onDisablePosition;

            m_playerCam.transform.position += movePosition;

            m_playerCam.SetActive(true);
        }

        if (m_virtualCam != null)
        {
            m_virtualCam.m_Lens.FieldOfView = _currentFieldOfView;
        }
    }

    private void OnDisable()
    {
        _onDisablePosition = transform.position;

        if(m_playerCam != null)
        {
            m_playerCam.SetActive(false);
        }
    }

    private void Start()
    {
        InitCamera();
    }

    private void Update()
    {
        CameraMovement();
    }

    private void RayPosition()
    {
        Ray mouseRayposition = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(mouseRayposition, out RaycastHit hit, 100))
        {
            Vector3 rayPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            Vector3 moveDir = (rayPosition - transform.position).normalized;

            float distanceTolook = Vector3.Distance(transform.position, rayPosition);

            if(distanceTolook > m_maxDistance)
            {
                rayPosition = transform.position + moveDir * m_maxDistance;
            }

            m_lookTrans.transform.position = Vector3.Lerp(m_lookTrans.transform.position, rayPosition, 5.0f * Time.deltaTime);

        }
    }

    private void CameraMovement()
    {
        bool isGetMouseButton_Right = Input.GetMouseButton(1);
        bool isGetKeyDown_LeftShift = Input.GetKey(KeyCode.LeftShift);

        if(isGetMouseButton_Right || isGetKeyDown_LeftShift)
        {
            SetTransposer(2f);

            if(isGetMouseButton_Right && isGetKeyDown_LeftShift)
            {
                m_virtualCam.Follow = m_maxTrans.transform;
                m_virtualCam.LookAt = m_maxTrans.transform;
            }
            else if (isGetMouseButton_Right)
            {
                m_virtualCam.Follow = m_targetTrans.transform;
                m_virtualCam.LookAt = m_targetTrans.transform;
            }
            else if (isGetKeyDown_LeftShift)
            {
                RayPosition();
                m_virtualCam.Follow = m_lookTrans.transform;
                m_virtualCam.LookAt = m_lookTrans.transform;
            }
        }
        else
        {
            SetTransposer(0f);
            m_virtualCam.Follow = transform;
            m_virtualCam.LookAt = transform;
        }
    }

    private void InitCamera()
    {
        m_playerCam = Instantiate(m_virtualCameraPrefab);

        m_playerCam.transform.rotation = Quaternion.Euler(51f, 0f, 0f);

        m_virtualCam = m_playerCam.GetComponent<CinemachineVirtualCamera>();

        m_virtualCam.Follow = transform;
        m_virtualCam.LookAt = transform;

        _currentFieldOfView = m_virtualCam.m_Lens.FieldOfView;

        var doNothing = m_virtualCam.GetCinemachineComponent<CinemachineComposer>();

        if (doNothing != null)
        {
            Destroy(doNothing);
        }

        _transposer = m_virtualCam.GetCinemachineComponent<CinemachineTransposer>();

        if (_transposer != null)
        {
            _transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            _transposer.m_FollowOffset = m_CinemachineVirtualCameraFollowOffSet;

            SetTransposer(0f);
        }
    }

    private void SetTransposer(float Damping)
    {
        _transposer.m_XDamping = Damping;
        _transposer.m_YDamping = Damping;
        _transposer.m_ZDamping = Damping;
    }

    private IEnumerator DeathCameraZoom()
    {
        float cameraFieldOfView = m_virtualCam.m_Lens.FieldOfView;

        float time = 0f;

        while (m_virtualCam.m_Lens.FieldOfView > _minView)
        {
            time += _zoomInSpeed * Time.deltaTime;

            m_virtualCam.m_Lens.FieldOfView = Mathf.Lerp(cameraFieldOfView,
                _minView, time * time);

            yield return null;
        }
    }

}
