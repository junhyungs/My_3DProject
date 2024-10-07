using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Boss/Mother/Init")]
public class Mother_Init : Conditional
{
    private TaskStatus _status;
    private bool _isInitialize;

    public override void OnStart()
    {
        if (!_isInitialize)
        {
            _status = TaskStatus.Success;

            _isInitialize = true;
        }
        else
        {
            _status = TaskStatus.Failure;
        }
    }

    public override TaskStatus OnUpdate()
    {
        return _status;
    }
}
