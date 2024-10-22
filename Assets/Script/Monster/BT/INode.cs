using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    public enum State
    {
        Running,
        Success,
        Fail
    }

    public State Evaluate();
}