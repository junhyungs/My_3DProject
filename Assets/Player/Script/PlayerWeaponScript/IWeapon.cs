using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void InitWeapon(GameObject hitRangeObject);
    public void UseWeapon(bool isCharge, GameObject hitRange);

}
