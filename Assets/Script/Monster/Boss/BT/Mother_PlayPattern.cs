using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Boss/Mother/PlayPattern")]
public class Mother_PlayPattern : Action
{
    private ForestMother _mother;
    private ForestMotherProperty _property;
    private ForestMotherVine _vine;

    private IMotherPattern _currentPattern;

    public override void OnAwake()
    {
        _mother = Owner.gameObject.GetComponent<ForestMother>();
        _vine = _mother.transform.GetComponentInChildren<ForestMotherVine>();

        _property = _mother.Property;
    }

    public override void OnStart()
    {
        _currentPattern = _property.CurrentPattern;

        _currentPattern.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        _currentPattern.OnUpdate();

        if (_currentPattern.IsRunning())
        {
            return TaskStatus.Running;
        }

        return TaskStatus.Success;
    }

    public override void OnEnd()
    {
        _currentPattern.OnEnd();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if(_currentPattern is Mother_Hyper)
        {
            Mother_Hyper hyper = _currentPattern as Mother_Hyper;

            hyper.Colliding(other);
        }
    }
}
