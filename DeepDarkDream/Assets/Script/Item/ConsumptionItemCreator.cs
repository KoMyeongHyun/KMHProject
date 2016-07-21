using System;
using System.Collections.Generic;

class ConsumptionItemCreator : ItemCreator
{
    public override Item CreateItem(Dictionary<string, object> _info)
    {
        int id = (int)_info["id"];
        string name = (string)_info["name"];
        ItemType type = (ItemType)_info["type"];
        string targetName = (string)_info["targetName"];
        string funcName = (string)_info["funcName"];
        float effect = (float)_info["effect"];

        Item item = new Item(id, name, type);
        item.USE = new ConsumptionItem(targetName, funcName, effect);

        return item;
    }
}
