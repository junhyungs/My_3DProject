using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SlimeMoveStateBehaviour : StateMachineBehaviour
{
    private SlimeBehaviour _slime;
    private Vector3 _movePosition;
    private NavMeshAgent _agent;
    private List<Vector3> _walkPositionList;
    private List<Vector3> _randomPositionList;

    private int _gridSize;
    private float _minDistance;
    private int _currentIndex = 0;

    private LayerMask _targetLayer;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetComponent(animator);

        _slime.CanMove = false;

        if (_slime.CheckPlayer)
        {
            _agent.stoppingDistance = 3f;

            _movePosition = _slime.PlayerObject.transform.position;
        }
        else
        {
            _agent.stoppingDistance = 0f;

            Patrol();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_movePosition);

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

            _gridSize = 20;
            _minDistance = 5f;
            _targetLayer = LayerMask.GetMask("Player", "Default");
        }
    }

    private void Patrol()
    {
        if(_currentIndex == 0)
        {
            _randomPositionList = new List<Vector3>(GetRandomPositionList());

            _walkPositionList = new List<Vector3>(GetMovePositionList(_randomPositionList));

            if(_walkPositionList.Count > 0)
            {
                _currentIndex++;

                _movePosition = _walkPositionList[_currentIndex];
            }
        }

        float remainingDistance = Vector3.Distance(_slime.transform.position, _movePosition);

        if(remainingDistance <= 0.1f)
        {
            _currentIndex++;

            if(_currentIndex < _walkPositionList.Count)
            {
                _movePosition = _walkPositionList[_currentIndex];
            }
            else
            {
                _currentIndex = 0;

                Patrol();
            }
        }
    }

    private List<Vector3> GetMovePositionList(List<Vector3> randomPositionList)
    {
        List<Vector3> removeList = new List<Vector3>();

        Collider[] colliders = Physics.OverlapSphere(_slime.transform.position, _gridSize, _targetLayer);

        foreach(var target in colliders)
        {
            Renderer targetRenderer = target.gameObject.GetComponent<Renderer>();

            if(targetRenderer != null)
            {
                Bounds targetBounds = targetRenderer.bounds;

                for(int i = 0; i < randomPositionList.Count; i++)
                {
                    Vector3 pos = randomPositionList[i];

                    if (targetBounds.Contains(pos))
                    {
                        removeList.Add(pos);
                    }
                }

                if(removeList.Count > 0)
                {
                    foreach(var removeTarget in removeList)
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

        vector3list.Add(_slime.transform.position);

        _slime.SetGrid(_slime.transform.position, _gridSize);//µð¹ö±ë ÄÚµå

        while (vector3list.Count < 5 && currentLoop < maxLoop)
        {
            float randomPositionX = Random.Range(-_gridSize / 2, _gridSize / 2);
            float randomPositionZ = Random.Range(-_gridSize / 2, _gridSize / 2);

            Vector3 randomPosition = new Vector3(randomPositionX + _slime.transform.position.x, _slime.transform.position.y
                , randomPositionZ + _slime.transform.position.z);

            if (Vector3.Distance(randomPosition, vector3list[vector3list.Count - 1]) > _minDistance)
            {
                vector3list.Add(randomPosition);
            }

            currentLoop++;
        }

        return vector3list;
    }

}
