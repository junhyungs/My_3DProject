using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("Monster/Ghoul/CanAttack")]
public class GhoulAction_CanAttack : Conditional 
{
    private Ghoul_BT _ghoul;
    private NavMeshAgent _agent;

    public override void OnAwake()
    {
        _ghoul = Owner.gameObject.GetComponent<Ghoul_BT>();

        _agent = _ghoul.GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        if(!_ghoul.CheckPlayer || _ghoul.IsReturn)
        {
            return TaskStatus.Failure;
        }

        if (_ghoul.IsAttack)
        {
            return TaskStatus.Running;
        }

        Transform playerTransform = _ghoul.PlayerObject.transform;

        float currentDistance = Vector3.Distance(_ghoul.transform.position,
            playerTransform.position);

        if(currentDistance <= _agent.stoppingDistance)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
