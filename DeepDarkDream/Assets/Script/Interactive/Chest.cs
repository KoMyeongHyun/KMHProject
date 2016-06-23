using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour
{
    bool openChest;

    [SerializeField]
    private GameObject item;

	// Use this for initialization
	void Start ()
    {
        openChest = false;
    }
	
    void OpenChest()
    {
        if(openChest == false)
        {
            gameObject.GetComponent<Animation>().Play();
            openChest = true;

            GameObject i = Instantiate(item);
            i.transform.position = transform.position;
        }
    }
}
