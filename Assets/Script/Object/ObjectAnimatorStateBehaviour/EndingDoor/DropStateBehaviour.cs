using UnityEngine;

public class DropStateBehaviour : StateMachineBehaviour
{
    private EndingDoor _door;
    private Transform _endingDoorTransform;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_door == null)
        {
            _door = animator.GetComponent<EndingDoor>();

            _endingDoorTransform = _door.transform;
        }

        UIManager.Instance.ItemInteractionUI(_endingDoorTransform, _door.UIPosition, ObjectName.OpenUI);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIManager.Instance.HideItemInteractionUI(_endingDoorTransform, ObjectName.OpenUI);
    }
}
