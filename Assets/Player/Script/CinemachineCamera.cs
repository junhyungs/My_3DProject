using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCamera : MonoBehaviour
{
    [Header("PlayerCameraPrefab")]
    [SerializeField] private GameObject m_virtualCameraPrefab;

    private GameObject m_playerCam;

    private void Start()
    {
        InitCamera();
    }

    private void InitCamera()
    {
        m_playerCam = Instantiate(m_virtualCameraPrefab);

        m_playerCam.transform.rotation = Quaternion.Euler(40.773f, 133.04f, 0f);

        CinemachineVirtualCamera virtualCam = m_playerCam.GetComponent<CinemachineVirtualCamera>();

        virtualCam.Follow = transform;
        virtualCam.LookAt = transform;

        var doNothing = virtualCam.GetCinemachineComponent<CinemachineComposer>();

        if (doNothing != null)
        {
            Destroy(doNothing);
        }

        var transPoser = virtualCam.GetCinemachineComponent<CinemachineTransposer>();

        if (transPoser != null)
        {
            transPoser.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            transPoser.m_FollowOffset = new Vector3(-16f, 20f, 16.5f);
            transPoser.m_XDamping = 3f;
            transPoser.m_YDamping = 3f;
            transPoser.m_ZDamping = 3f;
        }
    }

}
