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
    private Sprite icon;
    private int id;
    private string name;
    private ItemType type;
    private string targetName;
    private string funcName;
    private float effect;

    public Item() { }
    public Item(Sprite _icon, string _name, int _id, ItemType _type, string _targetName, string _funcName, float _effect)
    {
        //itemIcon = Resources.Load<Sprite>("" + iconName);
        icon = _icon;
        name = _name;
        id = _id;
        type = _type;
        targetName = _targetName;
        funcName = _funcName;
        effect = _effect;
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
    public string TARGET_NAME
    {
        set { targetName = value; }
        get { return targetName; }
    }
    public string FUNC_NAME
    {
        set { funcName = value; }
        get { return funcName; }
    }
    public float EFFECT
    {
        set { effect = value; }
        get { return effect; }
    }
}
