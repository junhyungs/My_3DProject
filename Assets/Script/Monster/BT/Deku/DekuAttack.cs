using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekuAttack : INode
{
    private DekuBehaviour _deku;
    private Animator _animator;
    private bool _canAttack = true;

    public DekuAttack(DekuBehaviour deku)
    {
        _deku = deku;
        _animator = _deku.GetComponent<Animator>();
    }

    public INode.State Evaluate()
    {
        if(_canAttack)
        {
            _deku.IsAttack = true;

            _animator.SetTrigger("Attack");

            _deku.StartCoroutine(CoolTime());

            return INode.State.Success;
        }

        return INode.State.Fail;
    }

    private IEnumerator CoolTime()
    {
        _canAttack = false;

        yield return new WaitForSeconds(1.0f);

        _canAttack = true;
    }
}
