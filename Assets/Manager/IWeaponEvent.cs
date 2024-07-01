using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEvent
{
    public void AddWeaponDataEvent(bool isAddEvent, Action<float, float, Vector3, Vector3> callBack);
}
