using UnityEngine;
using System.Collections;

public class SaveData
{
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

    private int[] loadedStage = new int[10];
    public void SetLoadedStage(int _stage)
    {
        loadedStage[_stage] = 1;
    }
    public int GetLoadedStage(int _stage)
    {
        return loadedStage[_stage];
    }

    //임시 데이터
    public Vector3 nextPosition;
}
