using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShadowPlayer : MonoBehaviour
{
    private Player _player;

    Vector3 _previousPosition;
    Vector3 _previouspreviousPosition;
    Vector3 _savePosition;

    public Vector3 SavePosition => _savePosition;

    private void Update()
    {
        if(_player._moveController.IsGround == false)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if(distance > 1f)
        {
            _previouspreviousPosition = _previousPosition;
            _previousPosition = _player.transform.position;
            transform.position = _previouspreviousPosition;

            _savePosition = transform.position - (_player.transform.position - transform.position).normalized;
        }

        Rotation();
    }

    private void Rotation()
    {
        Vector3 rotationDirection = _player.transform.position - transform.position;

        if(rotationDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(rotationDirection);

            transform.rotation = rotation;
        }
    }

    public void InitializeShadowPlayer(Player player)
    {
        _player = player;

        gameObject.layer = LayerMask.NameToLayer("ShadowPlayer");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, 1f);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(_savePosition, 1f);

        Vector3 start = transform.position;

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Vector3 end = start + forward * 5;  

        Debug.DrawLine(start, end);
    }

}
