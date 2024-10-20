using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SlimeMoveStateBehaviour : StateMachineBehaviour
{
    private SlimeBehaviour _slime;
    private NavMeshAgent _agent;
    private NavMeshPath _agentPath;
    private List<Vector3> _walkPositionList;
    private List<Vector3> _randomPositionList;

    private int _gridSize;
    private int _currentIndex = 0;
    private float _minDistance;
    private float _maxSampleDistance;

    private Vector3 _movePosition;
    private LayerMask _targetLayer;
    private Bounds _myBounds;


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
            _agentPath = new NavMeshPath();

            _gridSize = 10;
            _minDistance = 5f;
            _maxSampleDistance = 0.1f;
            _targetLayer = LayerMask.GetMask("Player", "Default");
            _myBounds = _slime.transform.GetComponentInChildren<SkinnedMeshRenderer>().bounds;
        }
    }

    private void Patrol()
    {
        if(_currentIndex == 0)
        {
            _randomPositionList = new List<Vector3>(GetRandomPositionList());

            _walkPositionList = new List<Vector3>(GetMovePositionList(_randomPositionList));

            if(_walkPositionList.Count > 1)
            {
                _currentIndex++;

                _movePosition = _walkPositionList[_currentIndex];
            }
            else
            {
                return;
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

    private bool CanAgentMove(Vector3 targetPosition)
    {
        if(NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, _maxSampleDistance, NavMesh.AllAreas))
        {
            Vector3 spherePosition = new Vector3(targetPosition.x, targetPosition.y +
                _slime.transform.position.y, targetPosition.z);

            Collider[] colliders = Physics.OverlapSphere(spherePosition, _myBounds.extents.magnitude, _targetLayer);

            if(colliders.Length == 0)
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
