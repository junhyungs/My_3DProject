using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatCheckPlayer : INode
{
    private BatBehaviour _bat;
    private NavMeshAgent _agent;

    private float _radius;
    private LayerMask _targetLayer;

    public BatCheckPlayer(BatBehaviour bat)
    {
        _bat = bat;
        _agent = _bat.GetComponent<NavMeshAgent>();

        _radius = _bat.Spawn ? _bat.Data.SpawnTrackingDistance
            : _bat.Data.TrackingDistance;

        _targetLayer = LayerMask.GetMask("Player");
    }

    public INode.State Evaluate()
    {
        if (_bat.CheckPlayer)
        {
            return INode.State.Success;
        }

        Collider[] colliders = Physics.OverlapSphere(_bat.transform.position, _radius, _targetLayer);

        if(colliders.Length > 0)
        {
            _agent.stoppingDistance = _bat.Data.AgentStoppingDistance;

            _bat.PlayerObject = colliders[0].gameObject;

            _bat.CheckPlayer = true;

            return INode.State.Fail;
        }

        _agent.stoppingDistance = 0f;

        return INode.State.Fail;
    }
}
