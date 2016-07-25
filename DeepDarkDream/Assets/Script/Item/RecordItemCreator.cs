using System;
using System.Collections.Generic;

class RecordItemCreator : ItemCreator
{
    public override Item CreateItem(Dictionary<string, object> _info)
    {
        int id = (int)_info["id"];
        string name = (string)_info["name"];
        ItemType type = (ItemType)_info["type"];

        Item item = new Record(id, name, type, new DisuseItem());
        //item.USE = new DisuseItem();

        return item;
    }
}
