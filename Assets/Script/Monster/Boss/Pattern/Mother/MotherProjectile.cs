using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherProjectile : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Transform _playerTransform;

    private float _speed;
    private float _forcePower;

    private Vector3 _targetDirection;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        GameObject player = GameManager.Instance.Player;
        _playerTransform = player.transform;
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void Explosion()
    {

    }
}
