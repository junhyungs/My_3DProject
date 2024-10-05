using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISendVineEvent
{
    public void AddVineEvent(Action<Vine, float> callBack, bool isRegistered);
}
