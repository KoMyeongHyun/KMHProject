using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Inventory : MonoBehaviour
{
    private Dictionary<int, GameObject> itemSlots = new Dictionary<int, GameObject>();
    public Dictionary<int, Item> items = new Dictionary<int, Item>();

    public GameObject slots;
    public int x, y, interval;

    [SerializeField]
    private GameObject tooltip;

    void Awake()
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

        tooltip.transform.SetAsLastSibling();
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
                items[i] = _item;
                itemSlots[i].GetComponent<Slot>().AddItem();
                GameObject.FindGameObjectWithTag("Canvas").SendMessage("ShowCatchItem", items[i]);

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
	
    public void ShowTooltip(Vector3 toolPosition, Item item)
    {
        tooltip.SetActive(true);

        tooltip.transform.position = toolPosition
            + new Vector3(Screen.width * 0.04f, 0.0f, 0.0f);
        tooltip.transform.GetChild(0).GetComponent<Text>().text = item.NAME;
    }

    public void CloseTooltip()
    {
        tooltip.SetActive(false);
    }
}
