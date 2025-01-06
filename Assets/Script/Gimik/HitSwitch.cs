using System;
using System.Collections;
using UnityEngine;

public class HitSwitch : MonoBehaviour, IHitSwitch
{
    [Header("GameObject")]
    [SerializeField] private GameObject EventObject;

    public Action<HitSwitch> _swithAction;
    private float _moveTime = 4f;
    private Vector3 _initializeDoorPosition;

    private void Awake()
    {
        _initializeDoorPosition = EventObject.transform.position;
    }

    public void OnHitSwitch()
    {
        if(EventObject == null)
        {
            return;
        }

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
        Vector3 startPosition = _initializeDoorPosition;

        Vector3 endPosition = startPosition + moveDirection;

        if(Vector3.Distance(eventObject.transform.position, endPosition) < 0.01f)
        {
            yield break;
        }

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
