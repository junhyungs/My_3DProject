using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss_BaseState : MonoBehaviour
{
    public virtual void StateEnter() { }
    public virtual void StateUpdate() { }
    public virtual void StateExit() { }
    public virtual void StateTriggerEnter(Collider other) { }
    public virtual void StateTriggerExit(Collider other) { }
    public virtual void StateCollisionEnter(Collision collision) { }
    public virtual void StateCollisionExit(Collision collision) { }
}
