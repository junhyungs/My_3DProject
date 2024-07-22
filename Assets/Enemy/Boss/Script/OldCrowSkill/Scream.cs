using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scream : IPattern
{
    private OldCrow _oldCrow;

    public Scream(OldCrow oldCrow)
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
