using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Boss/Mother/CheckQueue")]
public class CheckCommandQueue : Conditional
{
    private ForestMotherProperty _property;
    private TaskStatus _checkTask;

    public override void OnAwake()
    {
        var mother = Owner.gameObject.GetComponent<ForestMother>();

        _property = mother.Property;
    }

    public override void OnStart()
    {
        if(_property.PatternQueue.Count == 0)
        {
            _checkTask = TaskStatus.Failure;

            return;
        }

        _checkTask = TaskStatus.Success;
    }

    public override TaskStatus OnUpdate()
    {
        return _checkTask;
    }
}
