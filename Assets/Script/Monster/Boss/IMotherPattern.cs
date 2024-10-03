using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMotherPattern
{
    public void InitialzePattern(ForestMother mother);
    public void PlayPattern();
    public bool IsPlay();
}
