using UnityEngine;
using System.Collections;

public class Obstacle_07 : Obstacle
{
	// Use this for initialization
	protected override void Start ()
    {
        base.Start();
        transform.GetChild(0).GetComponent<Animation>().wrapMode = WrapMode.Loop;
	}

    void OnCollisionStay(Collision col)
    {
        if (col.transform.tag == "Interact")
        {
            gameObject.GetComponent<Animation>()[animationName[0]].speed = 0.0f;
            transform.GetChild(0).GetComponent<Animation>()[animationName[1]].speed = 0.0f;
        }
    }
    void OnCollisionExit(Collision col)
    {
        if (col.transform.tag == "Interact")
        {
            gameObject.GetComponent<Animation>()[animationName[0]].speed = 1.0f;
            transform.GetChild(0).GetComponent<Animation>()[animationName[1]].speed = 1.0f;
        }
    }
}