using UnityEngine;
using System;
using System.Collections.Generic;


class ConsumptionItem : ItemUse
{
    private string targetName;
    private string funcName;
    private float effect;

    public ConsumptionItem(string _targetName, string _funcName, float _effect)
    {
        targetName = _targetName;
        funcName = _funcName;
        effect = _effect;
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

    public int Use()
    {
        //GameObject.FindGameObjectWithTag(_item.TARGET_NAME).SendMessage(_item.FUNC_NAME, _item.EFFECT);
        GameObject.FindGameObjectWithTag(targetName).SendMessage(funcName, effect);
        return -1;
    }
}
