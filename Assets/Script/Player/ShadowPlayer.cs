using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShadowPlayer : MonoBehaviour
{
    private Player _player;

    Vector3 _previousPosition;
    Vector3 _previouspreviousPosition;

    private void Update()
    {
        if(_player._moveController.IsGround == false)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if(distance > 0.2f)
        {
            _previouspreviousPosition = _previousPosition;

            _previousPosition = _player.transform.position;

            transform.position = _previouspreviousPosition;
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
    }

}
