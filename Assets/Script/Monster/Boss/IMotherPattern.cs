using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMotherPattern
{
    public void InitializePattern(ForestMother mother);
    public void PlayPattern();
    public void EndPattern();
    public bool IsPlay();
}
