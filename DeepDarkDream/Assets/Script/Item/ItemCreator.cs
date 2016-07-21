using System;
using System.Collections.Generic;

class ItemCreator
{
    public virtual Item CreateItem(Dictionary<string, object> _info)
    {
        int id = (int)_info["id"];
        string name = (string)_info["name"];
        ItemType type = (ItemType)_info["type"];
        
        Item item = new Item(id, name, type);
        item.USE = new DisuseItem();

        return item;
    }
}