using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : INode
{
    private BatBehaviour _bat;
    private Animator _animator;

    private bool _coolDown = false;

    public BatAttack(BatBehaviour bat)
    {
        _bat = bat;
        _animator = _bat.GetComponent<Animator>();
    }

    public INode.State Evaluate()
    {
        if (_coolDown)
        {
            return INode.State.Fail;
        }

        _bat.IsAttack = true;

        _animator.SetTrigger("Attack");

        _coolDown = true;

        _bat.StartCoroutine(CoolDown());

        return INode.State.Success;
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(5f);

        _coolDown = false;
    }

}
