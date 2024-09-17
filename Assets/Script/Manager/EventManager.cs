
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static EventManager instance;
    private PlayerAttackController attackController;
    private List<IDisableArrow> m_OnDisableGhoulArrow = new List<IDisableArrow>();
    private List<IHookPosition> m_GetHookPositionEvetn = new List<IHookPosition>();
    private List<IDisableMagicBullet> m_OnDisableMageBullet = new List<IDisableMagicBullet>();

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
        IHookPosition hookPosition = hookPositionEvent;
        hookPosition.HookPositionEvent(true, attackController.OnHookCollied);
        //m_GetHookPositionEvetn.Add(hookPositionEvent);    
    }

    public void RegisterDisableGhoulArrow(IDisableArrow disableArrowEvent)
    {
        m_OnDisableGhoulArrow.Add(disableArrowEvent);
    }

    public void RegisterDisableMageBullet(IDisableMagicBullet disableMageBulletEvent)
    {
        m_OnDisableMageBullet.Add(disableMageBulletEvent);
    }

    //RegisterEvent-----------------------------------------------------------------------------------

    //AddEvent----------------------------------------------------------------------------------------
   
    public void AddEvent_HookPositionEvent(bool addEvent, Action<Vector3, bool> callBack)
    {
        foreach (var evtObj in m_GetHookPositionEvetn)
        {
            evtObj.HookPositionEvent(addEvent, callBack);
        }
    }

    public void AddEvent_DisableGhoulArrowEvent(Action callBack)
    {
        foreach(var ghoulArrow in m_OnDisableGhoulArrow)
        {
            ghoulArrow.OnDisableArrow(callBack);
        }
    }

    public void AddEvent_DisableMageBulletEvent(Action callBack)
    {
        foreach(var mageBullet in m_OnDisableMageBullet)
        {
            mageBullet.OnDisableMagicBullet(callBack);
        }
    }
}
