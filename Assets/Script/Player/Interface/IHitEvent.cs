using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitEvent
{
    public void HitOverlapBox(bool isAddEvent, Action<bool> callBack);
}
