using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElevatorMove
{
    Up,
    Down
}

public class Elevator : MonoBehaviour
{
    [Header("UpKey")]
    [SerializeField] private GameObject _upKey;

    [Header("DownKey")]
    [SerializeField] private GameObject _downKey;

    [Header("MoveSpeed")]
    [SerializeField] private float _elevatorMoveSpeed;

    private Vector3 _upDirection;
    private Vector3 _downDirection;
    private Vector3 _startPosition;

    private ElevatorMove _currentMove;

    private void Start()
    {
        InitializeUpKey();

        InitializeDownKey();

        InverseDirection();
    }

    private void InverseDirection()
    {
        _upDirection = transform.InverseTransformDirection(Vector3.up);

        _downDirection = transform.InverseTransformDirection(Vector3.down);

        _startPosition = transform.position;
    }

    private void InitializeUpKey()
    {
        if(_upKey == null)
        {
            return;
        }

        _upKey.AddComponent<UpElevatorKey>().InitializeUpKey(this);

        SphereCollider triggerSphereCollider = _upKey.AddComponent<SphereCollider>();

        triggerSphereCollider.radius = 0.2f;

        triggerSphereCollider.isTrigger = true;
    }

    private void InitializeDownKey()
    {
        if(_downKey == null)
        {
            return;
        }

        _downKey.AddComponent<DownElevatorKey>().InitializeDownKey(this);

        SphereCollider triggerSphereCollider = _downKey.AddComponent<SphereCollider>();

        triggerSphereCollider.radius = 0.2f;

        triggerSphereCollider.isTrigger = true;
    }

    public IEnumerator CallElevator(ElevatorMove moveDirection)
    {
        switch(moveDirection)
        {
            case ElevatorMove.Up:
                yield return StartCoroutine(UpElevator());
                _currentMove = ElevatorMove.Down;
                break;
            case ElevatorMove.Down:
                yield return StartCoroutine(DownElevator());
                _currentMove = ElevatorMove.Up;
                break;
        }
    }

    public IEnumerator UpElevator()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, _startPosition.y
            + 7.1f, transform.position.z);

        if(transform.position.y >= targetPosition.y)
        {
            yield break;
        }

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 moveVector3 = _upDirection * _elevatorMoveSpeed * Time.deltaTime;

            transform.Translate(moveVector3);

            yield return null;
        }

        transform.position = targetPosition;


    }

    public IEnumerator DownElevator()
    {
        Vector3 targetPosition = _startPosition;

        if(transform.position.y <= targetPosition.y)
        {
            yield break;
        }

        while(Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 moveVector3 = _downDirection * _elevatorMoveSpeed * Time.deltaTime;

            transform.Translate(moveVector3);

            yield return null;
        }

        transform.position = targetPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameObject player = other.gameObject;

            player.transform.SetParent(transform);

            switch (_currentMove)
            {
                case ElevatorMove.Up:
                    StartCoroutine(UpElevator());
                    break;
                case ElevatorMove.Down:
                    StartCoroutine(DownElevator());
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameObject player = other.gameObject;

            player.transform.parent = null;
        }
    }
}