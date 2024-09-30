using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("Monster/Ghoul/CheckPlayer")]
public class GhoulConditional_CheckPlayer : Conditional
{
    private Ghoul_BT _ghoul;

    private float _radius;
    private LayerMask _targetLayer;

    public override void OnAwake()
    {
        _ghoul = Owner.gameObject.GetComponent<Ghoul_BT>();

        _radius = 10f;

        _targetLayer = LayerMask.GetMask("Player");
    }

    public override TaskStatus OnUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(_ghoul.transform.position, _radius, _targetLayer);

        if( colliders.Length > 0 )
        {
            _ghoul.PlayerObject = colliders[0].gameObject;

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
