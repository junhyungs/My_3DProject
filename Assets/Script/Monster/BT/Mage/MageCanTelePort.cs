using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCanTelePort : INode
{
    private MageBehaviour _mage;

    public MageCanTelePort(MageBehaviour mageBehaviour)
    {
        _mage = mageBehaviour;
    }

    public INode.State Evaluate()
    {
        if (!_mage.CheckPlayer)
        {
            return INode.State.Fail;
        }

        if (_mage.IsTeleporting)
        {
            _mage.IsTeleporting = false;

            return INode.State.Success;
        }

        return INode.State.Fail;
    }
}
