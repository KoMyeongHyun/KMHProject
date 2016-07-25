using UnityEngine;
using System.Collections;

public enum ItemType
{
    KIT,
    WEAPON,
    CONSUMPTION,
    RECORD
}

public class Item
{
    private ItemUse use;
    private Sprite icon;
    private int id;
    private string name;
    private ItemType type;
    //private string targetName;
    //private string funcName;
    //private float effect;

    public Item() { }
    //public Item(Sprite _icon, string _name, int _id, ItemType _type, string _targetName, string _funcName, float _effect)
    public Item(int _id, string _name, ItemType _type, ItemUse _use)
    {
        //use에 대한 설정 해줄 것
        //itemIcon = Resources.Load<Sprite>("" + iconName);
        //icon = _icon;
        id = _id;
        name = _name;
        type = _type;
        use = _use;
        //targetName = _targetName;
        //funcName = _funcName;
        //effect = _effect;
    }
    public Item(Item _item)
    {
        //use에 대한 설정 해줄 것
        id = _item.id;
        name = _item.name;
        type = _item.type;
        use = _item.use;
        icon = _item.icon;
        //targetName = _item.targetName;
        //funcName = _item.funcName;
        //effect = _item.effect;
    }

    public ItemUse USE
    {
        //set { use = value; }
        get { return use; }
    }
    public Sprite ICON
    {
        set { icon = value; }
        get { return icon; }
    }
    public int ID
    {
        set { id = value; }
        get { return id; }
    }
    public string NAME
    {
        set { name = value; }
        get { return name; }
    }
    public ItemType TYPE
    {
        set { type = value; }
        get { return type; }
    }
    //public string TARGET_NAME
    //{
    //    set { targetName = value; }
    //    get { return targetName; }
    //}
    //public string FUNC_NAME
    //{
    //    set { funcName = value; }
    //    get { return funcName; }
    //}
    //public float EFFECT
    //{
    //    set { effect = value; }
    //    get { return effect; }
    //}

    public virtual int Use()
    {
        return use.Use();
    }
}
