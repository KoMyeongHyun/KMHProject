using UnityEngine;
using System.Collections;

public enum Behaviors { IDLE, GUARD, COMBAT, FLEE };

public abstract class State
{
    public abstract void Enter(GameObject obj);

    public abstract void Execute(GameObject obj);

    public abstract void Exit(GameObject obj);

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
