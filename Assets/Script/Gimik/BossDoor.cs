using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    private Vector3 _initializeDoorPosition;
    private float _moveTime = 4f;

    private void Awake()
    {
        _initializeDoorPosition = transform.position;
    }

    public void OpenDoor()
    {
        Vector3 moveDirection = Vector3.down * 5f;

        StartCoroutine(DoorCoroutine(moveDirection));
    }

    public void CloseDoor()
    {
        Vector3 moveDirection = Vector3.up * 5f;

        StartCoroutine(DoorCoroutine(moveDirection));
    }

    private IEnumerator DoorCoroutine(Vector3 moveDirection)
    {
        Vector3 startPosition = _initializeDoorPosition;

        Vector3 endPosition = startPosition + moveDirection;

        if (Vector3.Distance(transform.position, endPosition) < 0.01f)
        {
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < _moveTime)
        {
            transform.position =
                Vector3.Lerp(startPosition, endPosition, elapsedTime / _moveTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = endPosition;
    }

}
