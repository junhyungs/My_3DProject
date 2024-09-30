using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

[TaskCategory("Monster/Ghoul/Ready")]
public class GhoulAction_Ready : Action
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
        _agent.stoppingDistance = 8f;

        _ghoul.CheckPlayer = true;

        return TaskStatus.Success;
    }
}
