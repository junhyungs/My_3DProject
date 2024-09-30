using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Monster/Ghoul/Die")]
public class GhoulAction_Death : Action
{
    private Ghoul_BT _ghoul;

    public override void OnAwake()
    {
        _ghoul = Owner.gameObject.GetComponent<Ghoul_BT>();
    }

    public override TaskStatus OnUpdate()
    {
        _ghoul.Die(_ghoul.SoulPosition, _ghoul._disableHandler);

        return TaskStatus.Success;
    }
}
