using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossPattern
{
    MegaDash,
    Egg,
    Scream
}

public interface IPattern
{
    public void SetData<T>(T data);
    public void UsePattern();
}
