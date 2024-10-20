using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhoulCanAttack : INode
{
    private GhoulBehaviour _ghoul;
    private Animator _animator;
    private NavMeshAgent _agent;

    private float _stopTrackingDistance;
    public GhoulCanAttack(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;
        _animator = _ghoul.GetComponent<Animator>();
        _agent = _ghoul.GetComponent<NavMeshAgent>();

        _stopTrackingDistance = 10f;
    }

    public INode.State Evaluate()
    {
        if (!_ghoul.CheckPlayer || _ghoul.IsReturn)
        {
            return INode.State.Fail;
        }

        if (_ghoul.IsAttack)
        {
            return INode.State.Running;
        }

        Transform playerTransform = _ghoul.PlayerObject.transform;

        float currentDistance = Vector3.Distance(_ghoul.transform.position, 
            playerTransform.position);

        if(currentDistance >= _stopTrackingDistance)
        {
            _ghoul.IsReturn = true;

            return INode.State.Fail;
        }
        else
        {
            if(currentDistance <= _agent.stoppingDistance)
            {
                _animator.SetBool("TraceWalk", false);

                return INode.State.Success;
            }
            else
            {
                return INode.State.Fail;
            }
        }
    }
}
