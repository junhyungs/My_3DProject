using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageStateBehaviour : StateMachineBehaviour
{
    private Mage m_mage;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_mage == null)
        {
            m_mage = animator.GetComponent<Mage>();
        }

        m_mage.State.ChangeState(MonsterState.TelePort);
    }
}
