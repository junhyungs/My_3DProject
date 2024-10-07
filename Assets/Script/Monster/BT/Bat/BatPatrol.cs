using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatPatrol : INode
{
    private BatBehaviour _bat;
    private NavMeshAgent _agent;
    private NavMeshPath _agentPath;
    private List<Vector3> _walkPositionList;
    private List<Vector3> _randomPositionList;

    private int _gridSize;
    private int _currentIndex = 0;
    private float _maxSampleDistance = 0.1f;
    private float _minDistance;

    private LayerMask _targetLayer;
    private Bounds _myBounds;

    public BatPatrol(BatBehaviour bat)
    {
        _bat = bat;
        _agent = _bat.GetComponent<NavMeshAgent>();
        _agentPath = new NavMeshPath();

        _gridSize = 20;
        _minDistance = 5f;
        _targetLayer = LayerMask.GetMask("Player", "Default");
        _myBounds = _bat.transform.GetComponentInChildren<SkinnedMeshRenderer>().bounds;
    }

    public INode.State Evaluate()
    {
        if (_bat.CheckPlayer)
        {
            _currentIndex = 0;

            return INode.State.Success;
        }

        if (_currentIndex == 0)
        {
            _randomPositionList = new List<Vector3>(GetRandomPositionList());

            _walkPositionList = new List<Vector3>(GetMovePositionList(_randomPositionList));

            if(_walkPositionList.Count > 0)
            {
                _currentIndex++;

                _agent.SetDestination(_walkPositionList[_currentIndex]);
            }
        }

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _currentIndex++;

            if(_currentIndex < _walkPositionList.Count)
            {
                _agent.SetDestination(_walkPositionList[_currentIndex]);
            }
            else
            {
                _agent.SetDestination(_bat.transform.position);

                _currentIndex = 0;

                return INode.State.Success;
            }
        }

        return INode.State.Running;
    }

    private bool CanAgentMove(Vector3 targetPosition)
    {
        if(NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, _maxSampleDistance, NavMesh.AllAreas))
        {
            Vector3 spherePosition = new Vector3(targetPosition.x, targetPosition.y + _bat.transform.position.y
                , targetPosition.z);

            Collider[] colliders = Physics.OverlapSphere(spherePosition, _myBounds.extents.magnitude, _targetLayer);

            if(colliders.Length == 0)
            {
                bool IsAgentMove = AgentPath(hit.position);

                return IsAgentMove;
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

        foreach(var randomPosition in  randomPositionList)
        {
            if (!CanAgentMove(randomPosition))
            {
                removeList.Add(randomPosition);
            }
        }

        if(removeList.Count > 0)
        {
            foreach(var removePosition in removeList)
            {
                randomPositionList.Remove(removePosition);
            }
        }

        return randomPositionList;
    }

    private List<Vector3> GetRandomPositionList()
    {
        List<Vector3> vector3list = new List<Vector3>();

        int maxLoop = 20;
        int currentLoop = 0;

        vector3list.Add(_bat.transform.position);

        _bat.SetGrid(_bat.transform.position, _gridSize);//µð¹ö±ë ÄÚµå

        while (vector3list.Count < 5 && currentLoop < maxLoop)
        {
            float randomPositionX = Random.Range(-_gridSize / 2, _gridSize / 2);
            float randomPositionZ = Random.Range(-_gridSize / 2, _gridSize / 2);

            Vector3 randomPosition = new Vector3(randomPositionX + _bat.transform.position.x, _bat.transform.position.y
                , randomPositionZ + _bat.transform.position.z);

            if (Vector3.Distance(randomPosition, vector3list[vector3list.Count - 1]) > _minDistance)
            {
                vector3list.Add(randomPosition);
            }

            currentLoop++;
        }

        return vector3list;
    }
}
