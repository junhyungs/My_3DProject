using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother_Slam : IMotherPattern
{
    public Mother_Slam(ForestMotherPattern pattern)
    {
        _pattern = pattern;
    }

    private ForestMotherPattern _pattern;
    private ForestMother _mother;

    public void InitialzePattern(ForestMother mother)
    {
        _mother = mother;   
    }

    public bool IsPlay()
    {
        return true;
    }

    public void PlayPattern()
    {
        
    }
}
