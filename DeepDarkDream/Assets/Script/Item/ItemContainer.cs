using System;
using System.Collections;
using System.Collections.Generic;

class ItemContainer
{
    private static ItemContainer instance = null;
    public static ItemContainer Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ItemContainer();
            }
            return instance;
        }
    }

    private Dictionary<int, Item> items = new Dictionary<int, Item>();
    private int dynamicCount = 10000;

    public bool AddItem(Item _item)
    {
        if(items.ContainsKey(_item.ID))
        {
            return false;
        }
        items.Add(_item.ID, _item);
        return true;
    }
    
    public bool RemoveItem(int _id)
    {
        if (items.ContainsKey(_id))
        {
            return items.Remove(_id);
        }
        return false;
    }

    //id에 해당하는 아이템을 복사 생성 후 새로운 id를 리턴
    //상속관계에 있는 아이템에 대해서는 아직 미구현
    public int CreateItem(int _id)
    {
        Item item = new Item(items[_id]);
        item.ID = dynamicCount++;
        AddItem(item);

        int changedID = item.ID;
        return changedID;
    }

    public Item GetItem(int _id)
    {
        if(items.ContainsKey(_id))
        {
            return items[_id];
        }
        return null;
    }

    public int GetItemsCount()
    {
        return items.Count;
    }
}
