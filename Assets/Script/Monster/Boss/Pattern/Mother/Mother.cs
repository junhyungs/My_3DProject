using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mother
{
    protected ForestMother _mother;
    protected ForestMotherProperty _property;
    protected Animator _animator;
    protected NavMeshAgent _agent;

    protected readonly int _slamTrigger = Animator.StringToHash("SlamSpin");
    protected readonly int _slamSlowTrigger = Animator.StringToHash("SlamSlow");
    protected readonly int _slamSlowBool = Animator.StringToHash("Slow");
    protected readonly int _hyperTrigger = Animator.StringToHash("HyperSpin");
    protected readonly int _hyperBool = Animator.StringToHash("Hyper");
}
