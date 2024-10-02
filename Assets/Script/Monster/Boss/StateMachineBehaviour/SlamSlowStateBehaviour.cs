using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SlamSlowStateBehaviour : StateMachineBehaviour
{
    private ForestMother _mother;
    private ForestMotherAnimationEvent _animationEvent;

    private const string _slam_slowRotation = "Slam_slowRotation";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetMotherComponent(animator);

        _mother.StartCoroutine(OnSpinIdleAnimation(stateInfo, true));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //TODO 여기서 코드로 회전시켜도 될 것 같음. 고려해봅시다.
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SpinIdleAnimation(stateInfo, false);
    }

    private void GetMotherComponent(Animator animator)
    {
        if (_mother == null)
        {
            _mother = animator.GetComponent<ForestMother>();
            _animationEvent = animator.GetComponent<ForestMotherAnimationEvent>();
        }
    }

    private IEnumerator OnSpinIdleAnimation(AnimatorStateInfo stateInfo, bool isActive)
    {
        yield return new WaitForSeconds(0.5f);

        SpinIdleAnimation(stateInfo, isActive);
    }

    private void SpinIdleAnimation(AnimatorStateInfo stateInfo, bool isActive)
    {
        if (stateInfo.IsName(_slam_slowRotation))
        {
            _animationEvent.ActiveSpinIdle(isActive);
        }
    }
}
