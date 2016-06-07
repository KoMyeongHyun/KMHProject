﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    //public Item item;
    public int slotNum;
    private Inventory inven;
    public Image itemImage;
    private int itemCount;
    
    private float lastClickedTime;
    private const float catchTime = 0.25f;

    // Use this for initialization
    void Start ()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        inven = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        itemImage.enabled = false;
        itemCount = 0;

        lastClickedTime = 0.0f;
    }

    public void AddItem()
    {
        if (inven.items[slotNum].itemName == null)
            return;
        
        itemImage.sprite = inven.items[slotNum].itemIcon;
        itemImage.enabled = true;

        AddItemCount(1);
    }

    private void UseItem()
    {
        Item item = inven.items[slotNum];

        if (item.itemName == null || item.itemTargetName == null || item.itemFuncName == null)
            return;

        switch(item.itemType)
        {
            case ItemType.CONSUMPTION:
                GameObject.FindGameObjectWithTag(item.itemTargetName).SendMessage(item.itemFuncName, item.itemEffect);
                AddItemCount(-1);
                break;
        }
    }

    private void AddItemCount(int count)
    {
        itemCount += count;
        if (itemCount > 1)
        {
            transform.GetChild(1).GetComponent<Text>().text = itemCount.ToString();
        }
        else if(itemCount == 1)
        {
            transform.GetChild(1).GetComponent<Text>().text = null;
        }
        else
        {
            //아이템 삭제
            inven.items.Remove(slotNum);
            inven.items.Add(slotNum, new Item());
            inven.CloseTooltip();
            itemImage.sprite = null;
            itemImage.enabled = false;
            itemCount = 0;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        float intervalTime = Time.time - lastClickedTime;
        if(intervalTime < catchTime)
        {
            UseItem();

            lastClickedTime = 0.0f;
            return;
        }
        else
        {
            Debug.Log(transform.localPosition);
        }

        lastClickedTime = Time.time;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //마우스를 가져다 대면 아이템의 정보가 뜨도록 설정할 것
        if (inven.items[slotNum].itemName != null)
        {
            inven.ShowTooltip(this.transform.position, inven.items[slotNum]);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inven.CloseTooltip();
    }
}
