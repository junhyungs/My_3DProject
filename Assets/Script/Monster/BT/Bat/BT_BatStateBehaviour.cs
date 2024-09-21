using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatAttackStateBehaviour : StateMachineBehaviour
{
    private BatBehaviour batBehaviour;
    private NavMeshAgent _agent;
    private Vector3 _currentPlayerPosition;

    private float _stoppingDistance;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(batBehaviour == null)
        {
            batBehaviour = animator.GetComponent<BatBehaviour>();
            _agent = batBehaviour.GetComponent<NavMeshAgent>();
        }

        _stoppingDistance = _agent.stoppingDistance;

        _currentPlayerPosition = batBehaviour.PlayerObject.transform.position;

        _agent.stoppingDistance = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_currentPlayerPosition);

        if(stateInfo.normalizedTime >= 0.1f && stateInfo.normalizedTime <= 0.5f)
        {
            _agent.acceleration = 100f;
        }
        else
        {
            _agent.acceleration = 8f;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");

        _agent.SetDestination(batBehaviour.transform.position);

        _agent.stoppingDistance = _stoppingDistance;

        batBehaviour.IsAttack = false;
    }
}
