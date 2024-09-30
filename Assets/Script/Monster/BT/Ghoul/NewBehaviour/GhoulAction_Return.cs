using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

[TaskCategory("Monster/Ghoul/Return")]
public class GhoulAction_Return : Action
{
    private Ghoul_BT _ghoul;
    private NavMeshAgent _agent;
    private Animator _animator;

    private Vector3 _startPosition;

    public override void OnAwake()
    {
        _ghoul = Owner.gameObject.GetComponent<Ghoul_BT>();

        _agent = _ghoul.GetComponent<NavMeshAgent>();

        _animator = _ghoul.GetComponent<Animator>();

        _startPosition = _ghoul.transform.position;
    }

    public override TaskStatus OnUpdate()
    {
        if (!_ghoul.CheckPlayer)
        {
            return TaskStatus.Failure;
        }

        _animator.SetBool("TraceWalk", true);

        _agent.stoppingDistance = 0f;

        _agent.SetDestination(_startPosition);

        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.SetDestination(_ghoul.transform.position);

            _ghoul.CheckPlayer = false;

            _animator.SetBool("TraceWalk", true);

            _ghoul.IsReturn = false;

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
