using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatchInfo : MonoBehaviour
{
    public void SetInfo(Item _item)
    {
        transform.GetChild(0).GetComponent<Text>().text = _item.NAME;
        transform.GetChild(1).GetComponent<Text>().text = "State";
        transform.GetChild(2).GetComponent<Text>().text = "Desc";

        StartCoroutine(CheckClick());
    }
	
	IEnumerator CheckClick ()
    {
        while(Input.GetMouseButtonUp(0) == false)
        {
            yield return null;
        }

        yield return null;

        while (Input.GetMouseButtonUp(0) == false)
        {
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
