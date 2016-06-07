using UnityEngine;
using System.Collections;

public class ChaseStopRange : MonoBehaviour
{
    private bool chaseStop = false;
    public bool ChaseStop { get{ return chaseStop; } }

    void OnEnable()
    {
        chaseStop = false;
    }

    public void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            chaseStop = true;
        }
    }
}
