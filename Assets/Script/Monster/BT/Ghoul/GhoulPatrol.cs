using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhoulPatrol : INode
{
    private GhoulBehaviour _ghoul;
    private Animator _animator;
    private NavMeshAgent _agent;
    private NavMeshPath _agentPath;

    private List<Vector3> _walkPositionList;
    private List<Vector3> _randomPositionList;
    private List<Vector3> _removeList;

    private int _gridSize;
    private int _currentIndex = 0;
    private float _minDistance;
    private float _maxSampleDistance;
    private bool _isCoolTime;

    private LayerMask _targetLayer;
    private Bounds _myBounds;

    public GhoulPatrol(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;
        _agent = _ghoul.GetComponent<NavMeshAgent>();
        _agentPath = new NavMeshPath();
        _animator = _ghoul.GetComponent<Animator>();

        _gridSize = 10;
        _minDistance = 5f;
        _maxSampleDistance = 0.1f;
        _isCoolTime = false;
        _targetLayer = LayerMask.GetMask("Default");
        _myBounds = _ghoul.transform.GetComponentInChildren<SkinnedMeshRenderer>().bounds;
        _removeList = new List<Vector3>();
    }

    public INode.State Evaluate()
    {
        if (_ghoul.CheckPlayer)
        {
            _currentIndex = 0;

            return INode.State.Success;
        }

        if(_currentIndex == 0)
        {
            _randomPositionList = new List<Vector3>(GetRandomPositionList());

            _walkPositionList = new List<Vector3>(GetMovePositionList(_randomPositionList));
            
            if (_walkPositionList.Count > 1)
            {
                _currentIndex++;

                _animator.SetBool("TraceWalk", true);

                _agent.SetDestination(_walkPositionList[_currentIndex]);
            }
            else
            {
                return INode.State.Running;
            }
        }

        if(_agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
        {
            if (!_isCoolTime)
            {
                _animator.SetBool("TraceWalk", false);

                _agent.SetDestination(_ghoul.transform.position);

                _ghoul.StartCoroutine(PatrolCoolTime());
            }
        }

        return INode.State.Running;
    }

    private IEnumerator PatrolCoolTime()
    {
        _isCoolTime = true;

        yield return new WaitForSeconds(3f);

        _isCoolTime = false;

        _currentIndex++;

        if(_currentIndex < _walkPositionList.Count)
        {
            _animator.SetBool("TraceWalk", true);

            _agent.SetDestination(_walkPositionList[_currentIndex]);
        }
        else
        {
            _currentIndex = 0;
        }
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
        foreach (var randomPosition in randomPositionList)
        {
            if (!CanAgentMove(randomPosition))
            {
                _removeList.Add(randomPosition);
            }
        }

        if (_removeList.Count > 0)
        {
            foreach (var removePosition in _removeList)
            {
                randomPositionList.Remove(removePosition);
            }
        }

        _ghoul.SetList(randomPositionList);
        _removeList.Clear();
        return randomPositionList;
    }

    private List<Vector3> GetRandomPositionList()
    {
        List<Vector3> vector3list = new List<Vector3>();

        int maxLoop = 20;
        int currentLoop = 0;

        vector3list.Add(_ghoul.transform.position);

        _ghoul.SetGrid(_ghoul.transform.position, _gridSize);//µð¹ö±ë ÄÚµå

        while (vector3list.Count < 5 && currentLoop < maxLoop)
        {
            float randomPositionX = Random.Range(-_gridSize / 2, _gridSize / 2);
            float randomPositionZ = Random.Range(-_gridSize / 2, _gridSize / 2);

            Vector3 randomPosition = new Vector3(randomPositionX + _ghoul.transform.position.x, _ghoul.transform.position.y
                , randomPositionZ + _ghoul.transform.position.z);

            if(Vector3.Distance(randomPosition, vector3list[vector3list.Count - 1]) > _minDistance)
            {
                vector3list.Add(randomPosition);
            }

            currentLoop++;
        }

        return vector3list;
    }
}
