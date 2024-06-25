using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void InitWeapon();
    public void UseWeapon(bool isCharge);
}
