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
    private readonly int _hyper = Animator.StringToHash("Hyper");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    #region SpinIdle
    //SpinIdle => Hyper
    public void StartSpinIdle() => _animator.SetBool(_spinIdle, true);
    public void StopSpinIdle() => _animator.SetBool(_spinIdle, false);
    public void ActiveSpinIdle(bool isActive)
    {
        _animator.SetBool(_spinIdle, isActive);
    }
    #endregion

    #region Hyper
    //Hyper 시작 -> HyperSpin(Trigger) Hyper 종료 -> Hyper(SetBool(false))
    public void StartHyper() => _animator.SetBool(_hyper, true);
    public void StopHyper() => _animator.SetBool(_hyper, false);    
    #endregion

    #region Lift
    public void UpperWeightZero() => _animator.SetLayerWeight(1, 0f);
    public void UpperWeight() => _animator.SetLayerWeight(1, 1f);
    #endregion

    #region Slam
    //Slam 시작 -> SlamSpin(Trigger)
    public void StartSlamRotation() => _animator.SetTrigger(_slamRotation);
    public void StartSlam() => _animator.SetTrigger(_slam);
    #endregion

    #region Shoot

    #endregion
}
