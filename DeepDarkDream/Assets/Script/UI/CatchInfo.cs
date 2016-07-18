using UnityEngine;
using System.Collections;
//using UnityEngine.EventSystems;

public class CatchInfo : MonoBehaviour
{
    private bool activity;
	// Use this for initialization
	void Start ()
    {
        activity = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(activity == true)
            {
                gameObject.SetActive(false);
                activity = false;
                return;
            }
            activity = true;
        }
	}
}
