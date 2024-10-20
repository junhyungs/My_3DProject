using System;
using System.Collections;
using UnityEngine;

public class HitSwitch : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject EventObject;

    public Action<HitSwitch> _swithAction;
    private float _moveTime = 4f;

    public void SwitchEvent()
    {
        OpenDoor(EventObject);

        _swithAction?.Invoke(this);
    }

    private void OpenDoor(GameObject eventObject)
    {
        Vector3 moveDirection = Vector3.down * 5f;

        StartCoroutine(DoorCoroutine(eventObject, moveDirection));
    }

    private IEnumerator DoorCoroutine(GameObject eventObject, Vector3 moveDirection)
    {
        Vector3 startPosition = eventObject.transform.position;

        Vector3 endPosition = startPosition + moveDirection;

        float elapsedTime = 0f;

        while(elapsedTime < _moveTime)
        {
            eventObject.transform.position = 
                Vector3.Lerp(startPosition, endPosition, elapsedTime / _moveTime);
            
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        eventObject.transform.position = endPosition;
    }
}
