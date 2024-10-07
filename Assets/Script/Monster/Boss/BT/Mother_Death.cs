using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Boss/Mother/Death")]
public class Mother_Death : Action
{
    private ForestMother _mother;

    public override void OnAwake()
    {
        _mother = Owner.gameObject.GetComponent<ForestMother>();
    }

    public override void OnStart()
    {
        _mother.Die();
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Failure;
    }
}
