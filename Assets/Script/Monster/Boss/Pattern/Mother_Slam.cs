using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother_Slam : IMotherPattern
{
    private ForestMother _mother;
    private ForestMotherProperty _property;
    private Animator _animator;

    private readonly int _slam = Animator.StringToHash("SlamSpin");

    public void InitializePattern(ForestMother mother)
    {
        if(_mother == null)
        {
            _mother = mother;
            _property = _mother.Property;

            _animator = _mother.gameObject.GetComponent<Animator>();

        }

        _property.IsPlaying = true;

        _animator.SetTrigger(_slam);
    }

    public bool IsPlay()
    {
        bool isPlaying = _property.IsPlaying;

        if (isPlaying)
        {
            return true;
        }

        return false;
    }

    public void PlayPattern()
    {
        Debug.Log("Slam 공격 진행중");
    }

    public void EndPattern()
    {
        Debug.Log("Slam 공격 종료");
    }
}
