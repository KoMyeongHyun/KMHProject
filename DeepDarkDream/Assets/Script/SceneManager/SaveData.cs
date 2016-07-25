﻿using UnityEngine;
using LitJson;
using System.Collections;

public class SaveData
{
    SaveData()
    {
        stageInfo = new StageInfo[10];
        stageInfo[1].loadedStage = 0;
        stageInfo[2].loadedStage = 0;
    }

    private static SaveData instance = null;
    public static SaveData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveData();
            }
            return instance;
        }
    }

    private int loadingStageLevel;
    public int LoadingStageLevel
    {
        set { loadingStageLevel = value; }
        get { return loadingStageLevel; }
    }

    private bool firstLoad = true;
    public bool FirstLoad
    {
        set { firstLoad = value; }
        get { return firstLoad; }
    }

    //임시 데이터
    private StageInfo[] stageInfo;
    public void SetLoadedStage(int _stage)
    {
        stageInfo[_stage].loadedStage = 1;
    }
    public int GetLoadedStage(int _stage)
    {
        return stageInfo[_stage].loadedStage;
    }
    public Vector3 GetStartPos(int _stage)
    {
        return stageInfo[_stage].startPos;
    }
    public void InitStageInfo()
    {
        Vector3 startPos = new Vector3();
        JsonData root = SettingParser.Instance.SettingData;
        JsonData data = root["player"]["startPosition"];
        for (int i = 1; i < data.Count; ++i)
        {
            startPos.x = float.Parse(data[i]["position"][0].ToString());
            startPos.y = float.Parse(data[i]["position"][1].ToString());
            startPos.z = float.Parse(data[i]["position"][2].ToString());

            stageInfo[i].startPos = startPos;
        }
    }
    
    struct StageInfo
    {
        public Vector3 startPos;
        public int loadedStage;
    }
}
