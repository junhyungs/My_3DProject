using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Boss/Mother/MakePattern")]
public class MakeQueue : Action
{
    private ForestMother _mother;
    private ForestMotherProperty _property;

    private TaskStatus _status = TaskStatus.Success;

    public override void OnAwake()
    {
        _mother = Owner.gameObject.GetComponent<ForestMother>();

        _property = _mother.Property;
    }

    public override void OnStart()
    {
        MakePattern();
    }

    public override TaskStatus OnUpdate()
    {
        return _status;
    }

    private void MakePattern()
    {

    }

}
