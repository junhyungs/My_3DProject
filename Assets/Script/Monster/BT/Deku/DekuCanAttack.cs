using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DekuCanAttack : INode
{
    private DekuBehaviour _deku;
    private NavMeshAgent _agent;
    private Animator _animator;

    public DekuCanAttack(DekuBehaviour deku)
    {
        _deku = deku;
        _agent = _deku.GetComponent<NavMeshAgent>();    
        _animator = _deku.GetComponent<Animator>();
    }

    public INode.State Evaluate()
    {
        if (!_deku.CheckPlayer)
        {
            return INode.State.Fail;
        }

        if (_deku.IsAttack)
        {
            return INode.State.Running;
        }

        Transform playerTransform = _deku.PlayerObject.transform;

        float distance = Vector3.Distance(playerTransform.position, _deku.transform.position);

        if(distance > 11f)
        {
            _deku.CheckPlayer = false;

            _deku.PlayerObject = null;

            _deku.IsReturn = true;

            _agent.stoppingDistance = 0f;

            return INode.State.Fail;
        }
        else if(distance <= _agent.stoppingDistance)
        {
            _agent.SetDestination(_deku.transform.position);

            _animator.SetBool("Walk", false);

            return INode.State.Success;
        }

        return INode.State.Fail;
    }
}
