using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombObject : ProjectileObject
{
    protected override void Awake()
    {
        base.Awake();
        isFire = false;
    }

    private void FixedUpdate()
    {
        if (isFire)
        {

        }
    }
}
