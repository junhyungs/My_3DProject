using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatPatrol : INode
{
    private BatBehaviour _bat;
    private NavMeshAgent _agent;
    private List<Vector3> _walkPositionList;
    private List<Vector3> _randomPositionList;

    private int _gridSize;
    private int _currentIndex = 0;
    private float _minDistance;
    private LayerMask _targetLayer;

    public BatPatrol(BatBehaviour bat)
    {
        _bat = bat;
        _agent = _bat.GetComponent<NavMeshAgent>();

        _gridSize = 20;
        _minDistance = 5f;
        _targetLayer = LayerMask.GetMask("Player", "Default");
    }
    public INode.State Evaluate()
    {
        if(_currentIndex == 0)
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

    private List<Vector3> GetMovePositionList(List<Vector3> randomPositionList)
    {
        List<Vector3> removeList = new List<Vector3>();

        Collider[] colliders = Physics.OverlapSphere(_bat.transform.position, _gridSize, _targetLayer);

        foreach (var target in colliders)
        {
            Renderer targetRenderer = target.gameObject.GetComponent<Renderer>();

            if (targetRenderer != null)
            {
                Bounds targetBounds = targetRenderer.bounds;

                for (int i = 0; i < randomPositionList.Count; i++)
                {
                    Vector3 pos = randomPositionList[i];

                    if (targetBounds.Contains(pos))
                    {
                        removeList.Add(pos);
                    }
                }

                if (removeList.Count > 0)
                {
                    foreach (var removeTarget in removeList)
                    {
                        randomPositionList.Remove(removeTarget);
                    }
                }
            }
        }

        return randomPositionList;
    }

    private List<Vector3> GetRandomPositionList()
    {
        List<Vector3> vector3list = new List<Vector3>();

        int maxLoop = 10;
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
