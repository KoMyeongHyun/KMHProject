using UnityEngine;
using System.Collections;

public class StateMachine
{

    private GameObject obj;
    private State curState;
    public State GetCurState { get { return curState; } }

    public StateMachine(GameObject _obj, State _state)
    {
        obj = _obj;
        curState = _state;
        curState.Enter(obj);
    }

    public void ChangeState(State newState)
    {
        curState.Exit(obj);

        curState = newState;

        curState.Enter(obj);
    }

    public void RunBehaviors()
    {
        curState.Execute(obj);
    }

    public void TriggerEnter(GameObject obj, Collider col)
    {
        curState.TriggerEnter(obj, col);
    }

    public void TriggerStay(GameObject obj, Collider col)
    {
        curState.TriggerStay(obj, col);
    }

    public void TriggerExit(GameObject obj, Collider col)
    {
        curState.TriggerExit(obj, col);
    }
}
