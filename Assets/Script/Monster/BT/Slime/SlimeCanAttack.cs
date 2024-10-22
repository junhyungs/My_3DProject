using UnityEngine;

public class SlimeCanAttack : INode
{
    private SlimeBehaviour _slime;

    private float _stopDistance;

    public SlimeCanAttack(SlimeBehaviour slime)
    {
        _slime = slime;
        _stopDistance = 3f;
    }

    public INode.State Evaluate()
    {
        if (!_slime.CheckPlayer)
        {
            return INode.State.Fail;
        }

        if (_slime.IsAttack || !_slime.CanMove)
        {
            return INode.State.Running;
        }

        Transform playerTransform = _slime.PlayerObject.transform;

        float distance = Vector3.Distance(playerTransform.position, _slime.transform.position);

        if(distance > _stopDistance)
        {
            return INode.State.Fail;
        }

        return INode.State.Success;
    }
}
