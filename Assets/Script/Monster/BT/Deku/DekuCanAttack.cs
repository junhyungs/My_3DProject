using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DekuCanAttack : INode
{
    private DekuBehaviour _deku;
    private NavMeshAgent _agent;
    private Animator _animator;

    private float _stopTrackingDistance;

    public DekuCanAttack(DekuBehaviour deku)
    {
        _deku = deku;
        _agent = _deku.GetComponent<NavMeshAgent>();    
        _animator = _deku.GetComponent<Animator>();

        _stopTrackingDistance = 20f;
    }

    public INode.State Evaluate()
    {
        if (!_deku.CheckPlayer || _deku.IsReturn)
        {
            return INode.State.Fail;
        }

        if (_deku.IsAttack)
        {
            return INode.State.Running;
        }

        Transform playerTransform = _deku.PlayerObject.transform;

        float distance = Vector3.Distance(playerTransform.position, 
            _deku.transform.position);

        if(distance >= _stopTrackingDistance)
        {
            _deku.IsReturn = true;

            return INode.State.Fail;
        }
        else
        {
            if(distance <= _agent.stoppingDistance)
            {
                _animator.SetBool("Walk", false);

                return INode.State.Success;
            }
            else
            {
                return INode.State.Fail;
            }
        }
    }
}
