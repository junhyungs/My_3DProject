using System.Collections.Generic;

public class SelectorNode : INode
{
    private List<INode> _childNode;

    public SelectorNode(List<INode> childNode)
    {
        _childNode = childNode;
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
                case INode.State.Fail:
                    continue;
                case INode.State.Running:
                    return INode.State.Running;
                case INode.State.Success:
                    return INode.State.Success;
            }
        }

        return INode.State.Fail;
    }    
}
