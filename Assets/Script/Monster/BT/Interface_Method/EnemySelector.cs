using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelector : INode
{
    private List<INode> _childNodeList;

    public EnemySelector(List<INode> childNodeList)
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
                    return INode.State.Success;
                case INode.State.Fail:
                    continue;
            }
        }

        return INode.State.Fail;
    }
}
