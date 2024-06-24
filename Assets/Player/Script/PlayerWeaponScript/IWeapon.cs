using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void InitWeapon(Vfx_Controller effectRange, GameObject hitRangeObject);
    public void UseWeapon(bool isCharge, Vfx_Controller effectRange, GameObject hitRange);
}
