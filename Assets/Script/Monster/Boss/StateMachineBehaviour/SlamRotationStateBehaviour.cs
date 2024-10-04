using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamRotationStateBehaviour : StateMachineBehaviour
{
    private ForestMother _mother;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_mother == null)
        {
            _mother = animator.GetComponent<ForestMother>();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("SlamRotation_3"))
        {
            var property = _mother.Property;

            property.IsPlaying = false;
        }
    }

}
