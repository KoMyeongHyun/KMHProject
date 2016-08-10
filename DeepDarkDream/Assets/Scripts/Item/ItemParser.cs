using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using System;

public class ItemParser : Parser
{
    //item 정보를 전부 읽은 다음 Record 정보를 읽기 위한 검사용
    private bool itemParserCompletion;

    void Awake()
    {
        loadingCompletion = false;
        itemParserCompletion = false;
    }

    public void LoadItem(int _stage)
    {
        //string path = GetPath("ItemInfo.xml");
        StringBuilder xmlName = new StringBuilder();
        xmlName.AppendFormat("ItemInfo{0}.xml", _stage);
        string path = GetPath(xmlName.ToString());
        StartCoroutine(ParseItem(path));

        //path = GetPath("Record.xml");
        xmlName.Length = 0;
        xmlName.AppendFormat("Record{0}.xml", _stage);
        path = GetPath(xmlName.ToString());
        StartCoroutine(ParseRecord(path));
    }

    XmlDocument LoadXML(WWW _www)
    {
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            StringReader reader = new StringReader(_www.text);
            reader.Read();
            xmlDoc.LoadXml(reader.ReadToEnd());
        }
        catch
        {
            xmlDoc.LoadXml(_www.text);
        }

        return xmlDoc;
    }

    IEnumerator ParseItem(string _path)
    {
        WWW www = new WWW(_path);
        yield return www;

        XmlDocument xmlDoc = LoadXML(www);

        XmlNodeList nodeList = xmlDoc.SelectNodes("Items");
        foreach(XmlNode node in nodeList)
        {
            if(node.Name.Equals("Items") == false || node.HasChildNodes == false)
            {
                break;
            }
            
            foreach(XmlNode child in node.ChildNodes)
            {
                //아이템 컨테이너에 아이템 정보 셋팅
                //xml 스키마 정보를 일정하게 유지하기 위해 일단 값이 없어도 받아온다.
                int id = int.Parse(child.Attributes.GetNamedItem("id").Value);
                string name = child.Attributes.GetNamedItem("name").Value;
                string type_str = child.Attributes.GetNamedItem("type").Value;
                ItemType type = (ItemType)Enum.Parse(typeof(ItemType), type_str, true);
                string targetName = child.Attributes.GetNamedItem("targetName").Value;
                string funcName = child.Attributes.GetNamedItem("funcName").Value;
                string effect_str = child.Attributes.GetNamedItem("effect").Value;
                float effect = ( effect_str == "" ? 0 : float.Parse(effect_str) );
                
                //Hashtable info = new Hashtable();
                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add("id", id);
                info.Add("name", name);
                info.Add("type", type);
                info.Add("targetName", targetName);
                info.Add("funcName", funcName);
                info.Add("effect", effect);

                Item item = ItemFactory.Instance.CreateItem(type, info);
                ItemContainer.Instance.AddItem(item);
            }
        }

        itemParserCompletion = true;
    }

    IEnumerator ParseRecord(string _path)
    {
        WWW www = new WWW(_path);
        yield return www;

        while(itemParserCompletion == false)
        {
            yield return null;
        }

        XmlDocument xmlDoc = LoadXML(www);

        XmlNodeList nodeList = xmlDoc.SelectNodes("Records");
        foreach(XmlNode node in nodeList)
        {
            if(node.Name.Equals("Records") == false || node.HasChildNodes == false)
            {
                break;
            }

            foreach(XmlNode child in node.ChildNodes)
            {
                int id = int.Parse(child.Attributes.GetNamedItem("id").Value);
                Item item = ItemContainer.Instance.GetItem(id);
                if(item == null)
                {
                    print("Record 등록 실패 Item 존재 안함");
                    continue;
                }

                Record record = item as Record;
                string content_str = child.Attributes.GetNamedItem("content").Value;
                StringBuilder content = new StringBuilder(content_str);
                record.CONTENT = content;
                Debug.Log(content);
            }
        }

        loadingCompletion = true;
    }
}
