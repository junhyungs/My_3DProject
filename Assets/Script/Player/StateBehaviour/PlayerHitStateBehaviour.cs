using UnityEngine;


public class PlayerHitStateBehaviour : StateMachineBehaviour
{
    private PlayerMoveController m_moveContorller;
    private int m_Hit_back = Animator.StringToHash("Hit_back");
    private int m_Hit_Recover = Animator.StringToHash("Hit_Recover");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_moveContorller == null)
        {
            m_moveContorller = animator.GetComponent<PlayerMoveController>();
        }
            
        if (stateInfo.shortNameHash == m_Hit_back)
        {
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
            animator.gameObject.layer = LayerMask.NameToLayer("Player");

            m_moveContorller.IsAction = true;
        }
    }
}
