using UnityEngine;
using System.Collections;

public enum ItemType
{
    KIT,
    WEAPON,
    CONSUMPTION,
    BOOK
}

public class Item
{
    public Sprite itemIcon;
    public string itemName;
    public int itemID;
    public ItemType itemType;
    public string itemTargetName;
    public string itemFuncName;
    public float itemEffect;
    //public GameObject itemModel;

    public Item() { }
    public Item(Sprite icon, string name, int id, ItemType type, string targetName, string funcName, float effect)
    {
        //itemIcon = Resources.Load<Sprite>("" + iconName);
        itemIcon = icon;
        itemName = name;
        itemID = id;
        itemType = type;
        itemTargetName = targetName;
        itemFuncName = funcName;
        itemEffect = effect;
    }
}
