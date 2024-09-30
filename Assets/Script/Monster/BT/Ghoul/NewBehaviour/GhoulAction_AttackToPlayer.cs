using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Monster/Ghoul/AttackToPlayer")]
public class GhoulAction_AttackToPlayer : Action
{
    private Ghoul_BT _ghoul;
    private Animator _animator;

    public override void OnAwake()
    {
        _ghoul = Owner.gameObject.GetComponent<Ghoul_BT>();

        _animator = _ghoul.GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        _ghoul.IsAttack = true;

        _animator.SetTrigger("Attack");

        return TaskStatus.Success;
    }
}
