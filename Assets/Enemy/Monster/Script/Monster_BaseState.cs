using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class Monster_BaseState
{
    public virtual void StateEnter() { }
    public virtual void StateUpdate() { }
    public virtual void StateExit() { }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void StateOnTriggerStay(Collider other) { }  
    public virtual void StateOnTriggerExit(Collider other) { }
    public virtual void OnCollisionEnter(Collision collision) { }

}
