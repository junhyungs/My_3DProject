using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMotherPattern
{
    public void InitializeOnAwake(ForestMother mother, 
        ForestMotherProperty property);
    public void OnStart();
    public void OnUpdate();
    public void OnEnd();
    public bool IsRunning();
}
