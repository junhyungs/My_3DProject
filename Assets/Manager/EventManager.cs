
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static EventManager instance;
    private IWeaponEvent m_SetWeaponDataEvent;
    private IOnColliderEvent m_ActiveTriggerColliderEvent;
    private IHitEvent m_OverlapBoxEvent;
    private List<IHookPosition> m_GetHookPositionEvetn = new List<IHookPosition>();

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

    //RegisterEvent-----------------------------------------------------------------------------------
    public void RegisterSetWeaponDataEvent(IWeaponEvent setWeaponDataEvent)
    {
        m_SetWeaponDataEvent = setWeaponDataEvent;
    }

    public void RegisterActiveTriggerColliderEvent(IOnColliderEvent onTriggerColliderEvent)
    {
        m_ActiveTriggerColliderEvent = onTriggerColliderEvent;
    }

    public void RegisterOverlapBoxEvent(IHitEvent overlapEvent)
    {
        m_OverlapBoxEvent = overlapEvent;
    }

    public void RegisterHookPositionEvent(IHookPosition hookPositionEvent)
    {
        m_GetHookPositionEvetn.Add(hookPositionEvent);
    }

    //RegisterEvent-----------------------------------------------------------------------------------

    //AddEvent----------------------------------------------------------------------------------------
   
    public void AddEvent_SetWeaponDataEvent(bool addEvent, Action<float, float, Vector3, Vector3> callBack)
    {
        m_SetWeaponDataEvent.AddWeaponDataEvent(addEvent, callBack);
    }

    public void AddEvent_ActiveTriggerColliderEvent(bool addEvent, Action callBack)
    {
        m_ActiveTriggerColliderEvent.OnCollider(addEvent, callBack);
    }

    public void AddEvent_DeActiveTriggerColliderEvent(bool addEvent, Action callBack)
    {
        m_ActiveTriggerColliderEvent.OffCollider(addEvent, callBack);
    }

    public void AddEvent_OverlapBoxEvent(bool addEvent, Action<bool> callBack)
    {
        m_OverlapBoxEvent.HitOverlapBox(addEvent, callBack);
    }

    public void AddEvent_HookPositionEvent(bool addEvent, Action<Vector3, bool> callBack)
    {
        foreach(var evtObj in m_GetHookPositionEvetn)
        {
            evtObj.HookPositionEvent(addEvent, callBack);
        }
    }
}
