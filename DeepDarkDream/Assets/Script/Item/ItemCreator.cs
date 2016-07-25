using System;
using System.Collections.Generic;

class ItemCreator
{
    public virtual Item CreateItem(Dictionary<string, object> _info)
    {
        //item 객체를 받아오는 것이 낫지 않을까
        int id = (int)_info["id"];
        string name = (string)_info["name"];
        ItemType type = (ItemType)_info["type"];
        
        Item item = new Item(id, name, type, new DisuseItem());
        //item.USE = new DisuseItem();

        return item;
    }
}