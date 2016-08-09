using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour
{
    [SerializeField]
    private GameObject item;

    private const int DUMMY_ID = 3939393;
    private bool openChest;

    private Animation ani;
    private DroppedItem createdItem;
    private int cloneID;

	// Use this for initialization
	void Start ()
    {
        openChest = false;

        ani = gameObject.GetComponent<Animation>();
    }
	
    void OpenChest()
    {
        if(openChest == false)
        {
            openChest = true;

            //layer 기본 값으로 변경
            gameObject.layer = 0;
            
            //프리팹으로 존재하고 동적으로 생성해줘야하는 오브젝트의 경우
            //id를 새로 생성된 아이템 id로 변경
            GameObject clone = Instantiate(item);
            clone.transform.position = transform.position;
            createdItem = clone.GetComponent<DroppedItem>();
            cloneID = createdItem.ItemID;
            createdItem.ItemID = DUMMY_ID;
            
            StartCoroutine(CheckAnimation());
        }
    }
    
    IEnumerator CheckAnimation()
    {
        ani.Play();

        while(ani.isPlaying)
        {
            yield return null;
        }

        int id = ItemContainer.Instance.CreateItem(cloneID);
        createdItem.ItemID = id;
    }
}
