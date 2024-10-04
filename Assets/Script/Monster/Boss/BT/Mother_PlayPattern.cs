using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Boss/Mother/PlayPattern")]
public class Mother_PlayPattern : Action
{
    private ForestMother _mother;
    private ForestMotherProperty _property;

    private IMotherPattern _currentPattern;

    public override void OnAwake()
    {
        _mother = Owner.gameObject.GetComponent<ForestMother>();

        _property = _mother.Property;
    }

    public override void OnStart()
    {
        _currentPattern = _property.CurrentPattern;

        _currentPattern.InitializePattern(_mother);
    }

    public override TaskStatus OnUpdate()
    {
        _currentPattern.PlayPattern();

        if (_currentPattern.IsPlay())
        {
            return TaskStatus.Running;
        }

        return TaskStatus.Success;
    }

    public override void OnEnd()
    {
        _currentPattern.EndPattern();
    }
}
