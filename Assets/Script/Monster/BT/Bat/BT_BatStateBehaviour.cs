using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatAttackStateBehaviour : StateMachineBehaviour
{
    private BatBehaviour batBehaviour;
    private NavMeshAgent _agent;

    private Vector3 _currentPlayerPosition;
    private Vector3 _transformDirection;
    private Vector3 _boxSize;

    private LayerMask _targetLayer;
    private float _stoppingDistance;

    private bool _canAttack;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(batBehaviour == null)
        {
            batBehaviour = animator.GetComponent<BatBehaviour>();
            _agent = batBehaviour.GetComponent<NavMeshAgent>();

            _transformDirection = new Vector3(0f, 1f, 0f);
            _boxSize = new Vector3(1f, 1f, 1f);
            _targetLayer = LayerMask.GetMask("Player");
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
            _agent.speed = 10f;

            OverlapBox();
        }
        else
        {
            _agent.acceleration = 8f;
            _agent.speed = 4f;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");

        _canAttack = true;

        _agent.SetDestination(batBehaviour.transform.position);

        _agent.stoppingDistance = _stoppingDistance;

        batBehaviour.IsAttack = false;
    }

    private void OverlapBox()
    {
        if (_canAttack)
        {
            Vector3 boxPosition = batBehaviour.transform.position +
            batBehaviour.transform.TransformDirection(_transformDirection) +
            batBehaviour.transform.forward;

            Collider[] colliders = Physics.OverlapBox(boxPosition, _boxSize/2,
                batBehaviour.transform.rotation, _targetLayer);

            if (colliders.Length > 0)
            {
                IDamaged damaged = colliders[0].gameObject.GetComponent<IDamaged>();

                if (damaged != null)
                {
                    damaged.TakeDamage(batBehaviour.Power);
                }
            }

            _canAttack = false;
        }
    }
}
