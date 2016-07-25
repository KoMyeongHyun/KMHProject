using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;


class Record : Item
{
    private StringBuilder content;

    public Record(int _id, string _name, ItemType _type, ItemUse _use) 
        : base(_id, _name, _type, _use)
    { }

    public StringBuilder CONTENT
    {
        set { content = value; }
        get { return content; }
    }

    public override int Use()
    {
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().ShowCatchBook(this as Item);
        return 0;
    }
}
