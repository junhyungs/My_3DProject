using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeObjectMove : MonoBehaviour
{
    private float _maxDistance = 3f;
    [Header("Speed")]
    [SerializeField] private float _moveSpeed = 10f;
    private float _currentDistance;
    private float _y;
    private float _damage;

    private Vector3 _forward;
    private Vector3 _startPosition;

    private void OnEnable()
    {
        _currentDistance = 0f;
    }

    private void Update()
    {
        ObjectMove();
    }

    public void SetRotationValue(float y)
    {
        _y = y;

        Quaternion rotation = Quaternion.Euler(90f, _y, 0f);

        this.gameObject.transform.rotation = rotation;

        _forward = transform.forward;   

        _startPosition = transform.position;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void ObjectMove()
    {
        _currentDistance += _moveSpeed * Time.deltaTime;

        if(_currentDistance < _maxDistance)
        {
            transform.Translate(_forward * _moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-_forward * _moveSpeed * Time.deltaTime);

            float distance = Vector3.Distance(_startPosition, transform.position);

            if(distance < 0.1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamaged hit = other.gameObject.GetComponent<IDamaged>();

            if(hit != null)
            {
                hit.TakeDamage(_damage);
            }
        }
    }
}
