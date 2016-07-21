using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;


class Record : Item
{
    private StringBuilder content;

    public Record(int _id, string _name, ItemType _type) 
        : base(_id, _name, _type)
    { }

    public StringBuilder CONTENT
    {
        set { content = value; }
        get { return content; }
    }

    public override void Use()
    {
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().ShowCatchBook(this as Item);
    }
}
