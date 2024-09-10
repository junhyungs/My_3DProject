using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : INode
{
    private MageBehaviour _mage;

    public MageAttack(MageBehaviour mageBehaviour)
    {
        _mage = mageBehaviour;
    }

    public INode.State Evaluate()
    {
        if(_mage.AttackCount > 0)
        {
            if (!_mage.IsAttack)
            {
                _mage.IsTeleporting = true;

                _mage.AttackCount = 0;

                return INode.State.Success;
            }

            return INode.State.Fail;
        }

        _mage.Animator.SetTrigger("Attack");

        _mage.IsAttack = true;

        _mage.AttackCount = 1;

        return INode.State.Success;
    }
}
