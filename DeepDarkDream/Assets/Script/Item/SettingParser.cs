using UnityEngine;
using LitJson;
using System.Collections;

public class SettingParser : Parser
{
    private static SettingParser instance = null;
    public static SettingParser Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("SettingData");
                instance = obj.AddComponent<SettingParser>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private JsonData settingData;
    public JsonData SettingData
    {
        get { return settingData; }
    }

    void Awake()
    {
        loadingCompletion = false;
    }

    public void LoadSetting()
    {
        string fileName = "GameSetting.json";
        string path = GetPath(fileName);
        StartCoroutine(ParseSetting(path));
    }

    IEnumerator ParseSetting(string _path)
    {
        WWW www = new WWW(_path);
        yield return www;

        settingData = JsonMapper.ToObject(www.text);

        loadingCompletion = true;
    }
}
