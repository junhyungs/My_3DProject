using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POT_Cell : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private Vector3 _initializeLocalPosition;
    private Quaternion _initializeLocalRotation;

    private float _speed = 5f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        SetKinematic(true);

        _initializeLocalPosition = transform.localPosition;
        _initializeLocalRotation = transform.localRotation;
    }

    public void SetKinematic(bool use)
    {
        _rigidbody.isKinematic = use;
    }

    public void SetLayer(string layerName)
    {
        if (string.IsNullOrWhiteSpace(layerName))
        {
            return;
        }

        gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    public IEnumerator MoveAndRotateTowardsCoroutine(POT pot)
    {
        SetKinematic(true);

        while (true)
        {
            float distance = Vector3.Distance(transform.localPosition, _initializeLocalPosition);
            float angle = Quaternion.Angle(transform.localRotation, _initializeLocalRotation);

            bool isDistance = distance < 0.1f;
            bool isAngle = angle < 0.1f;

            if (!isDistance)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                _initializeLocalPosition, _speed * Time.deltaTime);
            }

            if (!isAngle)
            {
                float rotationSpeed = _speed * (angle / distance); 

                transform.localRotation = Quaternion.RotateTowards(transform.localRotation,
                _initializeLocalRotation, rotationSpeed * Time.deltaTime);
            } 

            if(isDistance && isAngle)
            {
                transform.localPosition = _initializeLocalPosition;
                transform.localRotation = _initializeLocalRotation;

                SetLayer("Default");
                pot.UnRegisterHashSet(this);

                yield break;
            }

            yield return null;
        }
    }
}
