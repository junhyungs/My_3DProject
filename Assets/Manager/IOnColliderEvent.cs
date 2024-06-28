using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnColliderEvent
{
    public void OnCollider(bool isAddEvent, Action callBack);
    public void OffCollider(bool isAddEvent, Action callBack);
}
