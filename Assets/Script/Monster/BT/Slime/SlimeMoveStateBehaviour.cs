using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SlimeMoveStateBehaviour : StateMachineBehaviour
{
    private SlimeBehaviour _slime;
    private Transform _moveTransform;
    private NavMeshAgent _agent;
    private List<Transform> _patrolList;
    private List<Transform> _shuffleList;

    private int _currentIndex;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetComponent(animator);

        _slime.CanMove = false;

        if (_slime.CheckPlayer)
        {
            _agent.stoppingDistance = 3f;

            _moveTransform = _slime.PlayerObject.transform;
        }
        else
        {
            _agent.stoppingDistance = 0f;

            Patrol();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_moveTransform.position);

        if (stateInfo.normalizedTime >= 0.3f && stateInfo.normalizedTime <= 0.6f)
        {
            _agent.speed = 7f;
        }
        else
        {
            _agent.speed = 0f;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _slime.CanRotation = true;

        _slime.CanMove = true;

        _agent.SetDestination(_slime.transform.position);
    }

    private void GetComponent(Animator animator)
    {
        if(_slime == null)
        {
            _slime = animator.GetComponent<SlimeBehaviour>();
            _agent = _slime.GetComponent<NavMeshAgent>();

            _patrolList = _slime.PatrolList;
            _currentIndex = 0;

            Shuffle();
        }
    }

    private void Patrol()
    {
        Initialize();

        _moveTransform = _shuffleList[_currentIndex];

        if(Vector3.Distance(_moveTransform.position, _slime.transform.position) <= 0.1f)
        {
            _currentIndex++;

            Initialize();

            _moveTransform = _shuffleList[_currentIndex];
        }
    }

    private void Initialize()
    {
        if (_currentIndex >= _shuffleList.Count)
        {
            Shuffle();

            _currentIndex = 0;
        }
    }

    private void Shuffle()
    {
        _shuffleList = new List<Transform>(_patrolList);

        for(int i = 0; i < _shuffleList.Count; i++)
        {
            Transform point = _shuffleList[i];

            int randomIndex = Random.Range(i, _shuffleList.Count);

            _shuffleList[i] = _shuffleList[randomIndex];

            _shuffleList[randomIndex] = point;
        }
    }
}
