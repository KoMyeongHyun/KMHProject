using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

class ItemFactory
{
    private static ItemFactory instance = null;
    public static ItemFactory Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ItemFactory();
            }
            return instance;
        }
    }

    private Dictionary<ItemType, ItemCreator> factory = new Dictionary<ItemType, ItemCreator>();

    public bool RegisterType(ItemType _type, ItemCreator _creator)
    {
        if (factory.ContainsKey(_type))
        {
            return false;
        }
        factory.Add(_type, _creator);
        return true;
    }

    public Item CreateItem(ItemType _type, Dictionary<string, object> _info)
    {
        if (factory.ContainsKey(_type) == false)
        {
            return null;
        }

        return factory[_type].CreateItem(_info);
    }
}
