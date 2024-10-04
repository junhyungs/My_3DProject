using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SlamSlowStateBehaviour : StateMachineBehaviour
{
    private ForestMother _mother;
    private ForestMotherAnimationEvent _animationEvent;

    private const string _slam_slow_idle = "Slam_slow_idle";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetMotherComponent(animator);

        if (stateInfo.IsName(_slam_slow_idle))
        {
            Debug.Log("코루틴 동작");
            _mother.StartCoroutine(OnSpinIdleAnimation(true));
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(_slam_slow_idle))
        {
            _mother.Property.IsPlaying = false;

            SpinIdleAnimation(false);
        }
    }

    private void GetMotherComponent(Animator animator)
    {
        if (_mother == null)
        {
            _mother = animator.GetComponent<ForestMother>();
            _animationEvent = animator.GetComponent<ForestMotherAnimationEvent>();
        }
    }

    private IEnumerator OnSpinIdleAnimation(bool isActive)
    {
        yield return new WaitForSeconds(0.5f);

        SpinIdleAnimation(isActive);
    }

    private void SpinIdleAnimation(bool isActive)
    {
        _animationEvent.ActiveSpinIdle(isActive);
    }
}
