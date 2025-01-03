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
        if(m_virtualCam != null)
        {
            m_virtualCam.m_Lens.FieldOfView = _currentFieldOfView;
        }

        if(m_playerCam != null)
        {
            Vector3 movePosition = transform.position - _onDisablePosition;

            m_playerCam.transform.position += movePosition;

            m_playerCam.SetActive(true);
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
        bool KeyDown = Input.anyKey;

        switch (KeyDown)
        {
            case true:
                if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftShift))
                {
                    m_virtualCam.Follow = m_maxTrans.transform;
                    m_virtualCam.LookAt = m_maxTrans.transform;
                }
                else if (Input.GetMouseButton(1))
                {
                    m_virtualCam.Follow = m_targetTrans.transform;
                    m_virtualCam.LookAt = m_targetTrans.transform;
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    RayPosition();
                    m_virtualCam.Follow = m_lookTrans.transform;
                    m_virtualCam.LookAt = m_lookTrans.transform;
                }
                break;
            case false:
                m_virtualCam.Follow = transform;
                m_virtualCam.LookAt = transform;
                break;
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

        var transPoser = m_virtualCam.GetCinemachineComponent<CinemachineTransposer>();

        if (transPoser != null)
        {
            transPoser.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            transPoser.m_FollowOffset = m_CinemachineVirtualCameraFollowOffSet;
            transPoser.m_XDamping = 2f;
            transPoser.m_YDamping = 2f;
            transPoser.m_ZDamping = 2f;
        }
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
