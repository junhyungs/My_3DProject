using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ForestMother : MonoBehaviour
{
    private Transform _playerTransform;
    private Vector3 _forward;

    void Start()
    {
        _playerTransform = GameManager.Instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = (_playerTransform.position - transform.position).normalized;

        targetDirection.y = 0f;

        float angle = Vector3.SignedAngle(transform.forward, targetDirection , Vector3.up);

        if(angle > 100)
        {
            
            Quaternion rotation = Quaternion.LookRotation(targetDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f * Time.deltaTime);
        }

    }

    private void OnDrawGizmos()
    {
        
    }
}
