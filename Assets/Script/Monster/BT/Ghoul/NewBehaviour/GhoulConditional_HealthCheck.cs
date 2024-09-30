using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Monster/Ghoul/HealthCheck")]
public class GhoulConditional_HealthCheck : Conditional
{
    private Ghoul_BT _ghoul;

    public override void OnAwake()
    {
        _ghoul = Owner.gameObject.GetComponent<Ghoul_BT>();
    }

    public override TaskStatus OnUpdate()
    {
        if (!_ghoul.CheckPlayer)
        {
            return TaskStatus.Running;
        }


        if(_ghoul.CurrentHealth <= 0 && !_ghoul.IsDead)
        {
            _ghoul.IsDead = true;

            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }
}
