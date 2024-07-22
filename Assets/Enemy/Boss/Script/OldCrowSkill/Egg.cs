using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : IPattern
{
    private OldCrow _oldCrow;
    public Egg(OldCrow oldCrow)
    {
        _oldCrow = oldCrow;
    }
    public void SetData<T>(T data)
    {
        
    }

    public void UsePattern()
    {
        
    }
}
