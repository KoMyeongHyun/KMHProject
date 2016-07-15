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

    public bool AddItem(Item _item)
    {
        if(items.ContainsKey(_item.ID))
        {
            return false;
        }

        items.Add(_item.ID, _item);
        return true;
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
