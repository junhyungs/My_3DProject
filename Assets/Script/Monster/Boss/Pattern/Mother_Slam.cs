using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother_Slam : Mother, IMotherPattern
{
    public void InitializeOnAwake(ForestMother mother, ForestMotherProperty property)
    {
        _mother = mother;

        _property = property;

        _animator = mother.GetComponent<Animator>();
    }

    public void OnStart()
    {
        _property.IsPlaying = true;

        _animator.SetTrigger(_slamTrigger);
    }

    public bool IsRunning()
    {
        bool isPlaying = _property.IsPlaying;

        if (isPlaying)
        {
            return true;
        }

        return false;
    }

    public void OnUpdate()
    {
        Debug.Log("Slam ���� ������");
    }

    public void OnEnd()
    {
        Debug.Log("Slam ���� ����");
    }
}
