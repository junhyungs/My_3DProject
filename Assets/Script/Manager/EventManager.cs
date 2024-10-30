using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static EventManager instance;
    private PlayerAttackController attackController;
    private List<IDisableMagicBullet> m_OnDisableMageBullet = new List<IDisableMagicBullet>();

    private IHookPosition _hook;

    public static EventManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new EventManager();
            }

            return instance;
        }
    }

    public void SetAttackContorller(PlayerAttackController controller)
    {
        attackController = controller;
    }

    //RegisterEvent-----------------------------------------------------------------------------------
    public void RegisterHookPositionEvent(IHookPosition hookPositionEvent)
    {
        if(_hook != null)
        {
            _hook = null;
        }

        _hook = hookPositionEvent;

        if(attackController != null)
        {
            _hook.HookPositionEvent(true, attackController.OnHookCollied);
        }
    }

    public void RegisterDisableMageBullet(IDisableMagicBullet disableMageBulletEvent)
    {
        m_OnDisableMageBullet.Add(disableMageBulletEvent);
    }

    public void AddEvent_DisableMageBulletEvent(Action callBack)
    {
        foreach(var mageBullet in m_OnDisableMageBullet)
        {
            mageBullet.OnDisableMagicBullet(callBack);
        }
    }
}
