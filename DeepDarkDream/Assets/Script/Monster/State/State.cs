using UnityEngine;
using System.Collections;

public enum Behaviors { IDLE, GUARD, COMBAT, FLEE };

public class State
{
    protected Animator ani;

    public virtual void Enter(GameObject obj)
    {

    }

    public virtual void Execute(GameObject obj)
    {

    }

    public virtual void Exit(GameObject obj)
    {

    }

    public virtual void TriggerEnter(GameObject obj, Collider col)
    {

    }

    public virtual void TriggerStay(GameObject obj, Collider col)
    {

    }

    public virtual void TriggerExit(GameObject obj, Collider col)
    {

    }
}
