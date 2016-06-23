using UnityEngine;
using System.Collections;

public class DanceController : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start ()
    {
        anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        GirlsDance();
	}

    void GirlsDance()
    {
        if( Input.GetKeyDown(KeyCode.Alpha1) )
        {
            anim.SetBool("Girls", true);
        }


    }
}
