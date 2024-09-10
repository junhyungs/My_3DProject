using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : INode
{
    private Func<INode.State> _state;

    public EnemyAction(Func<INode.State> state)
    {
        _state = state;
    }

    public INode.State Evaluate()
    {
        if(_state == null)
        {
            return INode.State.Fail;
        }
        else
        {
            return _state.Invoke();
        }
    }
}
