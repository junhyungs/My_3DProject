using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HookObject : ProjectileObject, IHookPosition
{
    private Action<Vector3> _hookEventHandler;

    protected override void Awake()
    {
        EventManager.Instance.RegisterHookPositionEvent(this);
    }

    public override void IsFire(bool fire)
    {
        isFire = fire;
    }

    //private void OnDisable()
    //{
    //    _hookEventHandler = null;
    //}

    void FixedUpdate()
    {
        if (isFire)
        {
            Vector3 movePos = transform.forward * (m_speed + 10) * Time.fixedDeltaTime;

            transform.position += movePos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hook")
        {
            _hookEventHandler?.Invoke(transform.position);
        }
    }

    public void HookPositionEvent(bool isAddEvent, Action<Vector3> callBack)
    {
        if (isAddEvent)
        {
            _hookEventHandler += callBack;
        }
        else
        {
            //_hookEventHandler -= callBack;
        }
    }
}
