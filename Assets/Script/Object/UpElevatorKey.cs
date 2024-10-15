using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpElevatorKey : MonoBehaviour
{
    private Elevator _elevatorReference;
    private ElevatorMove _moveDirection;

    private void Start()
    {
        _moveDirection = ElevatorMove.Up;
    }

    public void InitializeUpKey(Elevator elevator)
    {
        _elevatorReference = elevator;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(_elevatorReference.CallElevator(_moveDirection));
        }
    }
}
