using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderObject : MonoBehaviour
{
    [Header("RotationSpeed")]
    [SerializeField] private float _rotationSpeed;

    private Vector3 _rotateDirection;
    private Quaternion _rotation;

    void Start()
    {
        _rotateDirection = transform.InverseTransformDirection(Vector3.up);
    }

    void Update()
    {
        _rotation = Quaternion.Euler(_rotateDirection * _rotationSpeed * Time.unscaledDeltaTime);

        transform.localRotation = Quaternion.RotateTowards(transform.localRotation,
            transform.localRotation * _rotation, _rotationSpeed * Time.unscaledDeltaTime);
    }
}
