using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : INode
{
    private BatBehaviour _bat;
    private Animator _animator;

    private bool _canAttack = true;
    private LayerMask _targetLayer;

    public BatAttack(BatBehaviour bat, Animator animator)
    {
        _bat = bat;
        _animator = animator;

        _targetLayer = LayerMask.GetMask("Player");
    }

    public INode.State Evaluate()
    {
        if (_canAttack)
        {
            _bat.StartCoroutine(Attack());

            return INode.State.Success;
        }

        return INode.State.Running;
    }

    private IEnumerator Attack()
    {
        _bat.StartCoroutine(AttackCoolTime());

        _animator.SetTrigger("Attack");

        float moveDistance = 2f;
        Vector3 attackDirection = _bat.transform.forward;

        float moveTime = 0.5f;
        float startTime = Time.time;

        Vector3 startPosition = _bat.transform.position;
        Vector3 endPosition = startPosition + (attackDirection * moveDistance);

        while(Time.time < startTime + moveTime)
        {
            float time = (Time.time - startTime) / moveTime;
            _bat.transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
        }

        if(_bat.CurrentHP > 0)
        {
            Collider[] colliders = Physics.OverlapSphere(_bat.transform.position, 1f, _targetLayer);

            if(colliders.Length > 0)
            {
                IDamged hit = colliders[0].GetComponent<IDamged>();

                if (hit != null)
                {
                    hit.TakeDamage(_bat.CurrentPower);
                }
            }
        }
        
        _bat.transform.position = endPosition;
    }

    private IEnumerator AttackCoolTime()
    {
        _canAttack = false;

        yield return new WaitForSeconds(3f);

        _canAttack = true;
    }
}
