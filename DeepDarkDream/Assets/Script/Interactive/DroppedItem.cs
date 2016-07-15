using UnityEngine;
using System.Collections;

public class DroppedItem : MonoBehaviour
{
    //타입 선택에 따라 설정해야하는 값이 달라지도록 만들 것
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string itemName;
    [SerializeField] private int itemID;
    [SerializeField] private ItemType itemType;
    [SerializeField] private string itemTargetName;
    [SerializeField] private string itemFuncName;
    [SerializeField] private float itemEffect;

    private InputManager inputManager;
    private Transform cameraTrans;
    private Item item;
    private GameObject player;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpc;
    private static bool inProgress;

    public void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InputManager>();
        cameraTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        item = new Item(itemIcon, itemName, itemID, itemType, itemTargetName, itemFuncName, itemEffect);
        player = GameObject.FindGameObjectWithTag("Player");
        fpc = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        inProgress = false;
    }

    public void OnTriggerEnter(Collider col)
    {
    }

    public void OnTriggerStay(Collider col)
    {
        
        if (col.tag == "Player")
        {
            bool catchInfo = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().catchInfo.activeSelf;
            if (catchInfo || fpc.StopBehavior)
            {
                //inputManager.hideCursor();
                return;
            }

            if (Cast(col))
            {
                inputManager.ChangeCursor();

                if (Input.GetMouseButtonDown(0) && inProgress == false)
                {
                    //아이템 습득
                    inProgress = true;
                    Debug.Log("item get");
                    //아이템 정보 넘겨주기
                    GameObject.FindGameObjectWithTag("Inventory").SendMessage("AddItem", item);
                    inputManager.hideCursor();
                    Destroy(gameObject);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    //아이템 하나하나 먹을 수 있도록 마우스를 때면 초기화
                    inProgress = false;
                }
            }
            else
            {
                //하나라도 트루면 지우지 말 것
                inputManager.hideCursor();
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        //커서 없애기
        inputManager.hideCursor();
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
