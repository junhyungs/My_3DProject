using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpawnBatStateBehaviour : StateMachineBehaviour
{
    private SpawnBat m_Bat;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_Bat == null)
        {
            m_Bat = animator.GetComponent<SpawnBat>();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");

        m_Bat.State.ChangeState(MonsterState.Trace);
    }
}
