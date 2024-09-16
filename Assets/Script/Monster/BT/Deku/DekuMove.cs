using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DekuMove : INode
{
    private DekuBehaviour _deku;
    private NavMeshAgent _agent;
    private Animator _animator;

    public DekuMove(DekuBehaviour deku)
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

        Transform playerTransform = _deku.PlayerObject.transform;

        float distance = Vector3.Distance(playerTransform.position, _deku.transform.position);

        if(distance > _agent.stoppingDistance)
        {
            _animator.SetBool("Walk", true);

            _agent.SetDestination(playerTransform.position);

            return INode.State.Running;
        }

        return INode.State.Success;
    }
}
