using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Boss/Mother/ChangePattern")]
public class Mother_ChangePattern : Action
{
    private ForestMother _mother;
    private ForestMotherProperty _property;
    private TaskStatus _status;

    private int _listIndex;

    public override void OnAwake()
    {
        _mother = Owner.gameObject.GetComponent<ForestMother>();
        _property = _mother.Property;

        _listIndex = 0;
    }

    public override void OnStart()
    {
        if (_listIndex >= _property.PatternList.Count)
        {
            _listIndex = 0;
        }

        var newPattern = _property.PatternList[_listIndex];

        _listIndex++;

        _property.CurrentPattern = newPattern;

        _status = TaskStatus.Success;
    }

    public override TaskStatus OnUpdate()
    {
        return _status;
    }
}
