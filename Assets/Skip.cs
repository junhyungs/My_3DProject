using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Vector3 moveDirection = Vector3.forward;

            _rigidbody.position += moveDirection * 10f * Time.fixedDeltaTime;

            float roationAngle = 90f;
            Quaternion rotation = Quaternion.Euler(0f,roationAngle,0f);
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, rotation, Time.fixedDeltaTime * 10f);
        }
    }
}
