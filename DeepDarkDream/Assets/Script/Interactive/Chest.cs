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

            GameObject clone = Instantiate(item);
            clone.transform.position = transform.position;
            //코루틴 이용해서 애니메이션 끝나갈 때 만들어 줄 것
            //프리팹으로 존재하고 동적으로 생성해줘야하는 오브젝트의 경우
            //id를 새로 생성된 아이템 id로 변경
            int id = clone.GetComponent<DroppedItem>().ItemID;
            id = ItemContainer.Instance.CreateItem(id);
            clone.GetComponent<DroppedItem>().ItemID = id;
        }
    }
}
