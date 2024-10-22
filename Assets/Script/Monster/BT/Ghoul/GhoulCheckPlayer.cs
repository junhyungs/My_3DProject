using UnityEngine;
using UnityEngine.AI;

public class GhoulCheckPlayer : INode
{
    private GhoulBehaviour _ghoul;
    private NavMeshAgent _agent;

    private float _radius;
    private LayerMask _targetLayer;

    public GhoulCheckPlayer(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;
        _agent = _ghoul.GetComponent<NavMeshAgent>();

        _radius = _ghoul.Spawn ? _ghoul.Data.SpawnTrackingDistance : _ghoul.Data.TrackingDistance;

        _targetLayer = LayerMask.GetMask("Player");
    }

    public INode.State Evaluate()
    {
        if (_ghoul.CheckPlayer)
        {
            return INode.State.Success;
        }

        Collider[] colliders = Physics.OverlapSphere(_ghoul.transform.position, _radius, _targetLayer);

        if(colliders.Length > 0)
        {
            _agent.stoppingDistance = _ghoul.Data.AgentStoppingDistance;

            _ghoul.PlayerObject = colliders[0].gameObject;

            _ghoul.CheckPlayer = true;

            return INode.State.Fail;
        }

        return INode.State.Fail;
    }
}
