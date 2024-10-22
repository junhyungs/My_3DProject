using UnityEngine;
using UnityEngine.AI;

public class DekuCheckPlayer : INode
{
    private DekuBehaviour _deku;
    private Animator _animator;
    private NavMeshAgent _agent;

    private float _radius;
    private LayerMask _targetLayer;

    public DekuCheckPlayer(DekuBehaviour deku)
    {
        _deku = deku;
        _animator = _deku.GetComponent<Animator>();
        _agent = _deku.GetComponent<NavMeshAgent>();

        _radius = _deku.Spawn ? _deku.Data.SpawnTrackingDistance : _deku.Data.TrackingDistance;
        _targetLayer = LayerMask.GetMask("Player");
    }

    public INode.State Evaluate()
    {
        if (_deku.CheckPlayer)
        {
            return INode.State.Success;
        }

        Collider[] colliders = Physics.OverlapSphere(_deku.transform.position, _radius, _targetLayer);

        if(colliders.Length > 0 )
        {
            _agent.stoppingDistance = _deku.Data.AgentStoppingDistance;

            _animator.SetBool("Hide", false);

            _deku.PlayerObject = colliders[0].gameObject;

            _deku.CheckPlayer = true;

            return INode.State.Success;
        }

        return INode.State.Fail;
    }
}
