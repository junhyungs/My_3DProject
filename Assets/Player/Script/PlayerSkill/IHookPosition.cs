using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHookPosition
{
    public void HookPositionEvent(bool isAddEvent, Action<Vector3> callBack);
}
