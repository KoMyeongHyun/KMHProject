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

    private InputManager inputManager;
    private Transform cameraTrans;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpc;
    private static bool inProgress;

    public void Start()
    {
        inputManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InputManager>();
        cameraTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        fpc = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        inProgress = false;
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //아이템을 하나하나 먹을 수 있도록 마우스를 때면 초기화
            inProgress = false;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            //툴팁이 켜져있거나 아이템창을 열고 있는 상황
            bool catchInfo = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().catchInfo.activeSelf;
            bool catchRecord = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().CatchRecord.isActiveAndEnabled;
            if (catchInfo || catchRecord || fpc.StopBehavior)
            {
                //inputManager.HideCursor();
                return;
            }

            if (Cast(col))
            {
                inputManager.ChangeCursor();

                if (Input.GetMouseButtonDown(0) && inProgress == false)
                {
                    Item item = ItemContainer.Instance.GetItem(itemID);
                    if(item == null)
                    {
                        return;
                    }
                    item.ICON = itemIcon;

                    //아이템 습득
                    inProgress = true;
                    Debug.Log("item get");
                    //아이템 정보 넘겨주기
                    GameObject.FindGameObjectWithTag("Inventory").SendMessage("AddItem", item);
                    inputManager.HideCursor();
                    Destroy(gameObject);
                }
            }
            else
            {
                //하나라도 트루면 지우지 말 것
                inputManager.HideCursor();
            }
        }   
    }

    public void OnTriggerExit(Collider col)
    {
        //커서 없애기
        inputManager.HideCursor();
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
