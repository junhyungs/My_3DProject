using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Mother : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
}

public abstract class Forest_MotherState : Boss_BaseState
{
    protected Forest_Mother m_Mother;

    public Forest_MotherState(Forest_Mother mother)
    {
        m_Mother = mother;
    }
}



