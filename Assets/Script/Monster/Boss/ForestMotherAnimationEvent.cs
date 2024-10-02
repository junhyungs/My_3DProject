using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestMotherAnimationEvent : MonoBehaviour
{
    private Animator _animator;

    private readonly int _slam = Animator.StringToHash("Slam");
    private readonly int _slamRotation = Animator.StringToHash("SlamRotation");
    private readonly int _spinIdle = Animator.StringToHash("SpinIdle");
    private readonly int _shoot = Animator.StringToHash("Shoot");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    #region Lift
    public void UpperWeightZero()
    {
        _animator.SetLayerWeight(1, 0f);
    }

    public void UpperWeight()
    {
        _animator.SetLayerWeight(1, 1f);
    }
    #endregion

    #region Slam
    public void LowerWeightZero() => _animator.SetLayerWeight(2, 0f);
    public void LowerWeight() => _animator.SetLayerWeight(2, 1f);
    public void StartSlamRotation() => _animator.SetTrigger(_slamRotation);
    public void StartSlam() => _animator.SetTrigger(_slam);
    #endregion

    #region Shoot

    #endregion
}
