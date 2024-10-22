using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatCanAttack : INode
{
    private BatBehaviour _bat;
    private NavMeshAgent _agent;
    private float _stopTrackingDistance;

    public BatCanAttack(BatBehaviour bat)
    {
        _bat = bat;
        _agent = _bat.GetComponent<NavMeshAgent>();

        _stopTrackingDistance = _bat.Spawn ? _bat.Data.SpawnStopTrackingDistance
            : _bat.Data.StopTrackingDistance;
    }

    public INode.State Evaluate()
    {
        if (!_bat.CheckPlayer || _bat.IsReturn)
        {
            return INode.State.Fail;
        }

        if (_bat.IsAttack)
        {
            return INode.State.Running;
        }

        Transform playerTransform = _bat.PlayerObject.transform;

        float currentDistance = Vector3.Distance(_bat.transform.position,
            playerTransform.position);

        if(currentDistance > _stopTrackingDistance)
        {
            _bat.IsReturn = true;

            return INode.State.Fail;
        }
        else
        {
            if(currentDistance <= _agent.stoppingDistance)
            {
                return INode.State.Success;
            }
            else
            {
                return INode.State.Fail;
            }
        }
    }

}
