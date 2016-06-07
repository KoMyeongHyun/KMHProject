using UnityEngine;
using System.Collections;

public class Interact : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, -0.0001f, 0.0f));
	}
}
