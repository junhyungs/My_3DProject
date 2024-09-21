using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhoulPatrol : INode
{
    private GhoulBehaviour _ghoul;
    private Animator _animator;
    private NavMeshAgent _agent;
    private List<Vector3> _walkPositionList;
    private List<Vector3> _randomPositionList;

    private int _gridSize;
    private int _currentIndex = 0;
    private float _minDistance;
    private bool _isCoolTime;
    private LayerMask _targetLayer;

    public GhoulPatrol(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;
        _agent = _ghoul.GetComponent<NavMeshAgent>();   
        _animator = _ghoul.GetComponent<Animator>();

        _gridSize = 20;
        _minDistance = 5f;
        _isCoolTime = false;
        _targetLayer = LayerMask.GetMask("Default");
    }

    public INode.State Evaluate()
    {
        if(_currentIndex == 0)
        {
            _randomPositionList = new List<Vector3>(GetRandomPositionList());

            _walkPositionList = new List<Vector3>(GetMovePositionList(_randomPositionList));
            
            if (_walkPositionList.Count  > 0)
            {
                _currentIndex++;

                _animator.SetBool("TraceWalk", true);

                _agent.SetDestination(_walkPositionList[_currentIndex]);
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
            //++_currentIndex;

            //if(_currentIndex < _walkPositionList.Count)
            //{
            //    _animator.SetBool("TraceWalk", true);

            //    _agent.SetDestination(_walkPositionList[_currentIndex]);
            //}
            //else
            //{
            //    _animator.SetBool("TraceWalk", false);

            //    _agent.SetDestination(_ghoul.transform.position);

            //    _currentIndex = 0;

            //    return INode.State.Success;
            //}
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

    private List<Vector3> GetMovePositionList(List<Vector3> randomPositionList)
    {
        List<Vector3> removeList = new List<Vector3>();

        Collider[] colliders = Physics.OverlapSphere(_ghoul.transform.position, _gridSize, _targetLayer);

        foreach (var target in colliders)
        {
            Renderer targetRenderer = target.gameObject.GetComponent<Renderer>();

            if (targetRenderer != null)
            {
                Bounds targetBounds = targetRenderer.bounds;

                _ghoul.SetBounds(targetBounds); //����� �ڵ�

                for(int i = 0; i < randomPositionList.Count; i++)
                {
                    Vector3 pos = randomPositionList[i];

                    if(targetBounds.Contains(pos))
                    {
                        removeList.Add(pos);
                    }
                }

                if(removeList.Count > 0)
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

        vector3list.Add(_ghoul.transform.position);

        _ghoul.SetGrid(_ghoul.transform.position, _gridSize);//����� �ڵ�

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
