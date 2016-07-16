using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public Dictionary<int, GameObject> itemSlots = new Dictionary<int, GameObject>();
    public Dictionary<int, Item> items = new Dictionary<int, Item>();
    public GameObject slots;
    public int x, y, interval;

    public GameObject tooltip;
    public GameObject catchInfo;
    public GameObject catchBook;

    //private Image sourceImg;
    public Image bookImg;

    public AudioClip lockDoor;
    public AudioClip usedKey;
    public AudioClip openDoor;
    public AudioClip obstacle1;
    public AudioClip breath;
    public AudioClip zombieSound;

    //public bool catchInfoLock;

    void Start()
    {
        int slotAmount = 0;
        int initX = x;

        for (int i = 1; i < 5; ++i)
        {
            for(int j = 1; j < 5; ++j)
            {
                GameObject slot = Instantiate(slots);
                slot.transform.SetParent(this.gameObject.transform);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                slot.name = "slot" + i + "-" + j;
                slot.GetComponent<Slot>().slotNum = slotAmount;

                itemSlots.Add(slotAmount, slot);
                items.Add(slotAmount, new Item());

                x += interval;
                ++slotAmount;
            }
            x = initX;
            y -= interval;
        }

        //hide inven
        transform.position = new Vector3(5000.0f, 5000.0f);

        //sourceImg = catchInfo.GetComponent<Image>();
        
        //singleton 다음 스테이지로 넘어갔을 때 저장 값이 남아있다.
        SoundPool.Instance.AddSoundClip("LockDoor", lockDoor);
        SoundPool.Instance.AddSoundClip("UsedKey", usedKey);
        SoundPool.Instance.AddSoundClip("OpenDoor", openDoor);
        SoundPool.Instance.AddSoundClip("Obstacle1", obstacle1);
        SoundPool.Instance.AddSoundClip("호흡소리", breath);
        SoundPool.Instance.AddSoundClip("ZombieSound", zombieSound);
    }

    void OnDestory()
    {
        itemSlots.Clear();
        items.Clear();
    }

    void AddItem(Item _item)
    {
        //같은 이름의 아이템이 존재할 경우
        for (int i = 0; i < items.Count; ++i)
        {
            if(items[i].NAME == _item.NAME)
            {
                //Debug.Log(itemSlots[i].GetComponent<Slot>().itemCount);
                itemSlots[i].GetComponent<Slot>().AddItem();
                return;
            }
        }
        
        //함수 호출 시 items 빈 공간 검색 후 아이템 추가
        for (int i = 0; i < items.Count; ++i)
        {
            if(items[i].NAME == null)
            {
                //DB에서 가져오는 방법?
                //items[i] = Instantiate(GameObject.FindGameObjectWithTag("Empty")).GetComponent<Item>() ;

                items[i] = _item;
                
                //만약 책이라면 추가함수 실행
                if (_item.TYPE == ItemType.RECORD)
                {
                    ShowCatchBook(items[i]);
                }
                else
                {
                    ShowCatchInfo(items[i]);
                }
                itemSlots[i].GetComponent<Slot>().AddItem();
                break;
            }
        }
    }

    public bool SearchItem(string itemName)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].NAME == itemName)
            {
                return true;
            }
        }

        return false;
    }
	
    //인벤토리와 분리할 것
    public void ShowTooltip(Vector3 toolPosition, Item item)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = toolPosition + new Vector3(Screen.width*0.1f,-Screen.height*0.1f,0.0f);
        tooltip.transform.GetChild(0).GetComponent<Text>().text = item.NAME;
    }
    public void CloseTooltip()
    {
        tooltip.SetActive(false);
    }

    public void ShowCatchInfo(Item item)
    {
        catchInfo.SetActive(true);
        catchInfo.transform.localPosition = Vector3.zero;
        RectTransform catchRect = catchInfo.GetComponent<RectTransform>();
        catchRect.sizeDelta = new Vector2(400, 400);
        catchInfo.transform.GetChild(0).GetComponent<Text>().text = item.NAME;
        catchInfo.transform.GetChild(1).GetComponent<Text>().text = "State";
        catchInfo.transform.GetChild(2).GetComponent<Text>().text = "Desc";
        //inprogress true로 만들면
    }
    public void CloseCatchInfo()
    {
        catchInfo.SetActive(false);
        catchBook.SetActive(false);
    }
    public void ShowCatchBook(Item item)
    {
        //사이즈 설정

        catchBook.SetActive(true);
        catchBook.transform.localPosition = Vector3.zero;
        //이미지 설정
        RectTransform catchRect = catchInfo.GetComponent<RectTransform>();
        catchRect.sizeDelta = new Vector2(800, 600);

        Record record = item as Record;
        catchBook.transform.GetChild(0).GetComponent<Text>().text = record.CONTENT.ToString();
    }
}
