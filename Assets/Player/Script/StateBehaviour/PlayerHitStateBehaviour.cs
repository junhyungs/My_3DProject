using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerHitStateBehaviour : StateMachineBehaviour
{
    private int m_playerLayer;
    private PlayerMoveController m_moveContorller;

    private int m_Hit_back = Animator.StringToHash("Hit_back");
    private int m_Hit_idle = Animator.StringToHash("Hit_idle");
    private int m_Hit_Recover = Animator.StringToHash("Hit_Recover");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_moveContorller == null)
            animator.GetComponent<PlayerMoveController>();

        if(stateInfo.shortNameHash == m_Hit_back)
        {
            m_playerLayer = animator.gameObject.layer;
            animator.gameObject.layer = LayerMask.NameToLayer("HitPlayer");
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_moveContorller.IsAction = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.shortNameHash == m_Hit_Recover)
        {
            animator.gameObject.layer = m_playerLayer;
            m_moveContorller.IsAction = true;
        }
    }
}
