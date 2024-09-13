using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : INode
{
    private SlimeBehaviour _slime;
    private Animator _animator;

    private bool _canAttack = true;

    public SlimeAttack(SlimeBehaviour slime)
    {
        _slime = slime;
        _animator = _slime.GetComponent<Animator>();
    }

    public INode.State Evaluate()
    {
        if (_canAttack)
        {
            _animator.SetTrigger("Attack");

            _slime.StartCoroutine(CoolTime());

            return INode.State.Success;
        }

        return INode.State.Fail;
    }

    private IEnumerator CoolTime()
    {
        _canAttack = false;

        yield return new WaitForSeconds(3f);

        _canAttack = true;
    }
}
