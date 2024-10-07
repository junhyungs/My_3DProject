using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Boss/Mother/CanAttack")]
public class Mother_CanAttack : Conditional
{
    private ForestMother _mother;
    private ForestMotherProperty _property;

    public override void OnAwake()
    {
        _mother = Owner.gameObject.GetComponent<ForestMother>();

        _property = _mother.Property;
    }

    public override TaskStatus OnUpdate()
    {
        if (_property.CurrentHP <= 0)
        {
            return TaskStatus.Failure;
        }

        return TaskStatus.Success;
    }
}
