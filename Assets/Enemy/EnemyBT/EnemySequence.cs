using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySequence : INode
{
    private List<INode> _childNodeList;

    public EnemySequence(List<INode> childNodeList)
    {
        _childNodeList = childNodeList;
    }

    public INode.State Evaluate()
    {
        if(_childNodeList == null || _childNodeList.Count == 0)
        {
            return INode.State.Fail;
        }

        foreach(var childNode in _childNodeList)
        {
            switch (childNode.Evaluate())
            {
                case INode.State.Running:
                    return INode.State.Running;
                case INode.State.Success:
                    continue;
                case INode.State.Fail:
                    return INode.State.Fail;
            }
        }

        return INode.State.Success;

    }
}
