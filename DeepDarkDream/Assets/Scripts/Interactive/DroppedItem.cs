using UnityEngine;
using System.Collections;

public class DroppedItem : MonoBehaviour
{
    //타입 선택에 따라 설정해야하는 값이 달라지도록 만들 것
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private int itemID;
    public int ItemID
    {
        set { itemID = value; }
        get { return itemID; }
    }

    private ItemOutline outline;
    private Transform cameraTrans;

    public void Start()
    {
        outline = transform.GetChild(1).GetComponent<ItemOutline>();
        
        cameraTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            outline.SetOutline(0.008f);
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag != "Player")
        {
            return;
        }
        else if (Cast(col))
        {
            //inputManager.ChangeCursor();
            if(InputManager2.Instance.MouseButtonDown(UI_NONE_INPUT_KIND.DROPPED_ITEM))
            {
                Item item = ItemContainer.Instance.GetItem(itemID);
                if (item == null)
                {
                    return;
                }
                item.ICON = itemIcon;

                //아이템 습득
                Debug.Log("item get");
                //아이템 정보 넘겨주기
                GameObject.FindGameObjectWithTag("Inventory").SendMessage("AddItem", item);
                //inputManager.HideCursor();
                Destroy(gameObject);
            }
        }
        //else
        //{
            //하나라도 트루면 지우지 말 것
            //inputManager.HideCursor();
        //}
    }

    public void OnTriggerExit(Collider col)
    {
        //커서 없애기
        //inputManager.HideCursor();
        if (col.tag == "Player")
        {
            outline.SetOutline(0);
        }
    }

    private bool Cast(Collider col)
    {
        RaycastHit hit;
        float maxDistance = 10.0f;
        int layerMask = 1 << 12;
        //플레이어가 레이를 쏴서 아이템 레이어에 맞으면
        if (Physics.Raycast(cameraTrans.position, cameraTrans.forward, out hit, maxDistance, layerMask))
        {
            if( hit.transform.parent.GetComponent<DroppedItem>() == this )
            {
                return true;
            }
        }

        return false;
    }
}
