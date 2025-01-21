using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemachineCamera : MonoBehaviour
{
    [Header("PlayerCameraPrefab")]
    [SerializeField] private GameObject m_virtualCameraPrefab;

    [Header("CameraFollowOffset")]
    [SerializeField] private Vector3 m_CinemachineVirtualCameraFollowOffSet;

    private GameObject m_playerCam;
    private CinemachineVirtualCamera m_virtualCam;
    private CinemachineTransposer _transposer;
    private PlayerPlane _playerPlane;
    private Vector3 _onDisablePosition;

    private float _minView = 20f;
    private float _zoomInSpeed = 0.5f;
    private float _currentFieldOfView;

    private void Awake()
    {
        GameManager.Instance.RegisterDeathAction(DeathCameraZoom);

        _playerPlane = transform.GetComponentInChildren<PlayerPlane>();
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

            _transposer.m_XDamping = 0f;
            _transposer.m_YDamping = 0f;
            _transposer.m_ZDamping = 0f;

        }

        _playerPlane.VirtualCamera = m_virtualCam;
        _playerPlane.Transposer = _transposer;
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
