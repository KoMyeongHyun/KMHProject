using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using System;

public class ItemParser : MonoBehaviour
{
    void Awake()
    {
        //이전 씬에서 데이터 로딩할 때 진행할 것
        string path = GetPath("ItemInfo.xml");
        StartCoroutine(ParseItem(path));
        
        path = GetPath("Record.xml");
        StartCoroutine(ParseRecord(path));

        ItemFactory.Instance.RegisterType(ItemType.CONSUMPTION, new ConsumptionItemCreator());
        ItemFactory.Instance.RegisterType(ItemType.KIT, new ItemCreator());
        ItemFactory.Instance.RegisterType(ItemType.WEAPON, new ItemCreator());
        ItemFactory.Instance.RegisterType(ItemType.RECORD, new RecordItemCreator());
    }

    //XML을 다음과 같이 읽어 올 수도 있다.
    //TextAsset textXml = Resources.Load("파일경로", typeof(TextAsset)) as TextAsset;
    string GetPath(string _xmlName)
    {
        StringBuilder path = new StringBuilder();
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        //path.Append("file:///");
        path.AppendFormat("file:///{0}/{1}", Application.streamingAssetsPath, _xmlName);
#elif UNITY_ANDROID
        path.AppendFormat("jar:file://{0}!/assets/{1}", Application.dataPath, _xmlName);
#endif

        return path.ToString();
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
                float effect = float.Parse(child.Attributes.GetNamedItem("effect").Value);

                //Hashtable info = new Hashtable();
                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add("id", id);
                info.Add("name", name);
                info.Add("type", type);
                info.Add("targetName", targetName);
                info.Add("funcName", funcName);
                info.Add("effect", effect);
                //맞아 Record는 Record로 생성 해줘야 한다.
                Item item = ItemFactory.Instance.CreateItem(type, info);
                //Item item;
                //if (type == ItemType.RECORD)
                //{
                //    item = new Record();
                //}
                //else
                //{
                //    item = new Item();
                //}
                //item.ID = id;
                //item.NAME = name;
                //item.TYPE = type;
                //item.TARGET_NAME = targetName;
                //item.FUNC_NAME = funcName;
                //item.EFFECT = effect;

                ItemContainer.Instance.AddItem(item);
            }
        }
    }

    IEnumerator ParseRecord(string _path)
    {
        WWW www = new WWW(_path);
        yield return null;

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
    }
}
