using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Monster/Ghoul/RotateToPlayer")]
public class GhoulAction_RotateToPlayer : Action
{
    private Ghoul_BT _ghoul;
    private Transform _playerTransform;

    private Quaternion _rotation;

    private float _rotationSpeed;
    private float _angle;

    public override void OnAwake()
    {
        _ghoul = Owner.gameObject.GetComponent<Ghoul_BT>();

        _rotationSpeed = 20f;
    }

    public override TaskStatus OnUpdate()
    {
        _playerTransform = _ghoul.PlayerObject.transform;

        Vector3 rotateDirection = (_playerTransform.position - _ghoul.transform.position).normalized;

        _angle = Vector3.Angle(_ghoul.transform.forward, rotateDirection);

        if(_angle > 10f)
        {
            _rotation = Quaternion.LookRotation(rotateDirection);

            _ghoul.transform.rotation = Quaternion.Slerp(_ghoul.transform.rotation, _rotation
                , _rotationSpeed * Time.deltaTime);

            return TaskStatus.Running;
        }

        _ghoul.transform.rotation = Quaternion.LookRotation(rotateDirection);

        return TaskStatus.Success;
    }
}
