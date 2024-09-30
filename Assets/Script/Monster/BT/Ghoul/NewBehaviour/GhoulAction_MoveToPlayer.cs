using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

[TaskCategory("Monster/Ghoul/MoveToPlayer")]
public class GhoulAction_MoveToPlayer : Action
{
    private Ghoul_BT _ghoul;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _playerTransform;

    public override void OnAwake()
    {
        _ghoul = Owner.gameObject.GetComponent<Ghoul_BT>();

        _agent = _ghoul.GetComponent<NavMeshAgent>();

        _animator = _ghoul.GetComponent<Animator>();
    }
    
    public override TaskStatus OnUpdate()
    {
        if (_ghoul.IsReturn)
        {
            return TaskStatus.Failure;
        }

        _animator.SetBool("TraceWalk", true);

        _playerTransform = _ghoul.PlayerObject.transform;

        _agent.SetDestination(_playerTransform.position);

        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _animator.SetBool("TraceWalk", false);

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
