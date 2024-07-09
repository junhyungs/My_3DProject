using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitEvent : MonoBehaviour
{
    protected int m_key;
    protected Action m_HitAction;

    public abstract void OpenDoor(int key, Action hitAction);
}
