using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("MoveSpeed")]
    [SerializeField] private float _moveSpeed;

    [Header("Collider")]
    [SerializeField] private BoxCollider _upCollider;
    [SerializeField] private BoxCollider _downCollider;

    private Vector3 _upDirection = Vector3.up;
    private Vector3 _downDirection = Vector3.down;
    private Vector3 _startPosition;

    private float _maxHeight = 0f;
    private float _minHeight = -7.1f;
    private float _currentHeight;

    private bool _isUp;

    private void Start()
    {
        _upDirection = transform.InverseTransformDirection(Vector3.up);

        _downDirection = transform.InverseTransformDirection(Vector3.down);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(UpElevator());
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(DownElevator());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(CallElevator(true));
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(CallElevator(false));
        }
    }
    public IEnumerator CallElevator(bool currentCall)
    {
        if (currentCall)
        {
            yield return StartCoroutine(UpElevator());

            _upCollider.enabled = false;

            _isUp = false;
        }
        else
        {
            yield return StartCoroutine(DownElevator());

            _downCollider.enabled = false;

            _isUp = true;
        }
    }

    public IEnumerator UpElevator()
    {
        _upCollider.enabled = true;

        _downCollider.enabled = true;

        _startPosition = transform.position;

        _currentHeight = transform.position.y;

        while(_currentHeight < _maxHeight)
        {
            Vector3 upElevator = _upDirection * _moveSpeed * Time.deltaTime;

            transform.Translate(upElevator);

            _currentHeight = transform.position.y - _startPosition.y;

            yield return null;
        }

        _upCollider.enabled = false;
    }
    

    public IEnumerator DownElevator()
    {
        _upCollider.enabled = true;

        _downCollider.enabled = true;

        _startPosition = transform.position;

        _currentHeight = 0f;

        while (_currentHeight > _minHeight)
        {
            Vector3 upElevator = _downDirection * _moveSpeed * Time.deltaTime;

            transform.Translate(upElevator);

            _currentHeight = transform.position.y - _startPosition.y;

            yield return null;
        }

        _downCollider.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

        }
    }
}