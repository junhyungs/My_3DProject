using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

[TaskCategory("Monster/Ghoul/Patrol")]
public class GhoulAction_Patrol : Action
{
    private Ghoul_BT _ghoul;
    private Animator _animator;
    private NavMeshAgent _agent;
    private NavMeshPath _agentPath;

    private List<Vector3> _walkPositionList;
    private List<Vector3> _randomPositionList;

    private int _gridSize;
    private int _currentIndex;
    private float _minDistance;
    private float _maxSampleDistance;

    private LayerMask _targetLayer;
    private Bounds _myBounds;

    public override void OnAwake()
    {
        _ghoul = Owner.gameObject.GetComponent<Ghoul_BT>();
        _agent = _ghoul.GetComponent<NavMeshAgent>();
        _animator = _ghoul.GetComponent<Animator>();
        _agentPath = new NavMeshPath();

        _gridSize = 20;
        _minDistance = 5f;
        _maxSampleDistance = 0.1f;
        _targetLayer = LayerMask.GetMask("Default");
        _myBounds = _ghoul.transform.GetComponentInChildren<SkinnedMeshRenderer>().bounds;
    }

    public override TaskStatus OnUpdate()
    {
        if (_currentIndex == 0)
        {
            _randomPositionList = new List<Vector3>(GetRandomPositionList());

            _walkPositionList = new List<Vector3>(GetMovePositionList(_randomPositionList));

            if (_walkPositionList.Count > 0)
            {
                _currentIndex++;

                _animator.SetBool("TraceWalk", true);

                _agent.SetDestination(_walkPositionList[_currentIndex]);
            }
        }

        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _currentIndex++;

            if (_currentIndex < _walkPositionList.Count)
            {
                _animator.SetBool("TraceWalk", true);

                _agent.SetDestination(_walkPositionList[_currentIndex]);
            }
            else
            {
                _animator.SetBool("TraceWalk", false);

                _agent.SetDestination(_ghoul.transform.position);

                _currentIndex = 0;
            }
        }

        return TaskStatus.Running;
    }

    private bool CanAgentMove(Vector3 targetPosition)
    {
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, _maxSampleDistance, NavMesh.AllAreas))
        {
            Vector3 spherePosition = new Vector3(targetPosition.x, targetPosition.y +
                _ghoul.transform.position.y, targetPosition.z);

            Collider[] colliders = Physics.OverlapSphere(spherePosition, _myBounds.extents.magnitude, _targetLayer);

            if (colliders.Length == 0)
            {
                bool canAgentMove = AgentPath(hit.position);

                return canAgentMove;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private bool AgentPath(Vector3 hitPosition)
    {
        _agent.CalculatePath(hitPosition, _agentPath);

        return _agentPath.status == NavMeshPathStatus.PathComplete;
    }

    private List<Vector3> GetMovePositionList(List<Vector3> randomPositionList)
    {
        List<Vector3> removeList = new List<Vector3>();

        foreach (var randomPosition in randomPositionList)
        {
            if (!CanAgentMove(randomPosition))
            {
                removeList.Add(randomPosition);
            }
        }

        if (removeList.Count > 0)
        {
            foreach (var removePosition in removeList)
            {
                randomPositionList.Remove(removePosition);
            }
        }

        return randomPositionList;
    }

    private List<Vector3> GetRandomPositionList()
    {
        List<Vector3> vector3list = new List<Vector3>();

        int maxLoop = 10;
        int currentLoop = 0;

        vector3list.Add(_ghoul.transform.position);

        while (vector3list.Count < 5 && currentLoop < maxLoop)
        {
            float randomPositionX = Random.Range(-_gridSize / 2, _gridSize / 2);
            float randomPositionZ = Random.Range(-_gridSize / 2, _gridSize / 2);

            Vector3 randomPosition = new Vector3(randomPositionX + _ghoul.transform.position.x, _ghoul.transform.position.y
                , randomPositionZ + _ghoul.transform.position.z);

            if (Vector3.Distance(randomPosition, vector3list[vector3list.Count - 1]) > _minDistance)
            {
                vector3list.Add(randomPosition);
            }

            currentLoop++;
        }

        return vector3list;
    }
}
