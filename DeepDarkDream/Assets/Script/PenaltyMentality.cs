using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PenaltyMentality : MonoBehaviour
{
    private RawImage img;

	// Use this for initialization
	void Start ()
    {
        img = GetComponent<RawImage>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        img.SetAllDirty();
    }
}
