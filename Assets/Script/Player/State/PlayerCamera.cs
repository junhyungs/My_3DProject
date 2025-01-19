using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private const string _path = "Prefab/Camera/PlayerVirtualCam";

    private Vector3 _cameraFollowOffSet = new Vector3(0f, 10f, -8f);

    private void Start()
    {
        SetPlayerCamera();
    }

    private void SetPlayerCamera()
    {
        var cameraResource = Resources.Load<GameObject>(_path);

        var camera = Instantiate(cameraResource);

        camera.transform.rotation = Quaternion.Euler(51f, 0f, 0f);

        var virtualCameraComponent = camera.GetComponent<CinemachineVirtualCamera>();
        virtualCameraComponent.Follow = transform;
        virtualCameraComponent.LookAt = transform;

        var doNothing = virtualCameraComponent.GetCinemachineComponent<CinemachineComposer>();
        
        if(doNothing != null)
        {
            Destroy(doNothing);
        }

        var transPoser = virtualCameraComponent.GetCinemachineComponent<CinemachineTransposer>();

        if(transPoser != null)
        {
            transPoser.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            transPoser.m_FollowOffset = _cameraFollowOffSet;
            transPoser.m_XDamping = 2f;
            transPoser.m_YDamping = 2f;
            transPoser.m_ZDamping = 2f;
        }

    }

}
