using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MageTelePort : INode
{
    private MageBehaviour _mage;

    private float _maxTime;
    private float _startValue;
    private float _endValue;
    private float _teleportDistance;

    public MageTelePort(MageBehaviour mageBehaviour)
    {
        _mage = mageBehaviour;

        _maxTime = 2.5f;
        _startValue = 0.5f;
        _endValue = -0.3f;
        _teleportDistance = 10f;
    }

    public INode.State Evaluate()
    {
        _mage.StartCoroutine(TelePortCoroutine());

        return INode.State.Success;
    }

    private IEnumerator TelePortCoroutine()
    {
        yield return _mage.StartCoroutine(TelePort_In());

        yield return _mage.StartCoroutine(TelePort_Out());
    }

    private IEnumerator TelePort_In()
    {
        _mage.CanMove = false;

        _mage.Animator.SetTrigger("TelePort");

        float elapsedTime = 0f;

        while (elapsedTime < _maxTime)
        {
            elapsedTime += Time.deltaTime * 2f;

            float colorValue = Mathf.Lerp(_startValue, _endValue, elapsedTime / _maxTime);

            _mage.CopyMaterial.SetFloat("_Float", colorValue);

            yield return null;
        }
    }

    private IEnumerator TelePort_Out()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _teleportDistance;

        randomDirection.y = 0f;

        Transform playerTransform = _mage.PlayerObject.transform;

        Vector3 teleportPosition = playerTransform.position + randomDirection;

        NavMeshHit hit;

        if(NavMesh.SamplePosition(teleportPosition, out hit, _teleportDistance, NavMesh.AllAreas))
        {
            _mage.Agent.Warp(hit.position);

            Vector3 rotateDirection = (playerTransform.position - _mage.transform.position).normalized;

            rotateDirection.y = 0f;

            if(rotateDirection != Vector3.zero)
            {
                _mage.transform.rotation = Quaternion.LookRotation(rotateDirection);
            }
        }

        yield return new WaitForSeconds(0.5f);

        _mage.Animator.SetBool("TelePort_In", true);

        float elapsedTime = 0f;

        while (elapsedTime < _maxTime)
        {
            elapsedTime += Time.deltaTime;

            float colorValue = Mathf.Lerp(_endValue, _startValue, elapsedTime / _maxTime);

            _mage.CopyMaterial.SetFloat("_Float", colorValue);

            yield return null;
        }

        _mage.Animator.SetBool("TelePort_In", false);

        _mage.CanMove = true;
    }
}
