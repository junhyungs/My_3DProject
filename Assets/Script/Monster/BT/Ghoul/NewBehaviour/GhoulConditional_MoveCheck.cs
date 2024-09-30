using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Monster/Ghoul/CheckPlayer")]
public class GhoulConditional_MoveCheck : Conditional
{
    private Ghoul_BT _ghoul;
    private Transform _playerTransform;

    private float _stopTrackingDistance;

    public override void OnAwake()
    {
        _ghoul = Owner.gameObject.GetComponent<Ghoul_BT>();

        _stopTrackingDistance = 20f;
    }

    public override TaskStatus OnUpdate()
    {
        if (!_ghoul.CheckPlayer)
        {
            return TaskStatus.Failure;
        }

        _playerTransform = _ghoul.PlayerObject.transform;

        float distance = Vector3.Distance(_ghoul.transform.position, _playerTransform.position);

        if(distance > _stopTrackingDistance)
        {
            _ghoul.IsReturn = true;

            return TaskStatus.Failure;
        }

        return TaskStatus.Success;
    }
}
