using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : INode
{
    private List<INode> _childNode;

    public SequenceNode(List<INode> childNode)
    {
        _childNode=childNode;
    }

    public INode.State Evaluate()
    {
        bool list = _childNode == null || _childNode.Count == 0;

        if (list)
        {
            return INode.State.Fail;
        }

        foreach(var childNode in _childNode)
        {
            INode.State state = childNode.Evaluate();

            switch (state)
            {
                case INode.State.Success:
                    continue;
                case INode.State.Running:
                    return INode.State.Running;
                case INode.State.Fail:
                    return INode.State.Fail;
            }
        }

        return INode.State.Success;
    }
}
