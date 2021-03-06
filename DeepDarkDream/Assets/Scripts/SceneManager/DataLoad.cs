﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DataLoad : MonoBehaviour
{
    //const int SCREEN_WIDTH = 1280;

    [SerializeField]
    private GameObject staticDataLoad;

    private ItemParser itemParser;

    void Start()
    {
        itemParser = gameObject.GetComponent<ItemParser>();

        int level = SaveData.Instance.LoadingStageLevel;
        string sceneName = "Game" + level;
        StartCoroutine(ChangeSceneAndDisplayLoadingScreen(sceneName));
    }

    IEnumerator ChangeSceneAndDisplayLoadingScreen(string sceneName)
    {
        gameObject.SetActive(true);

        int loadingPercentage = 0;
        Text percentage = transform.GetChild(1).GetComponent<Text>();
        int gaugeSize = 0;
        RectTransform progressBar = transform.GetChild(2).GetComponent<RectTransform>();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.88f)
        {
            loadingPercentage = (int)(asyncLoad.progress * 100);
            percentage.text = loadingPercentage + "%";
            gaugeSize = (int)((int)STANDARD_SIZE.WIDTH * loadingPercentage * 0.01);
            progressBar.sizeDelta = new Vector2(gaugeSize, progressBar.sizeDelta.y);

            yield return null;
        }

        //각종 자원 로딩
        yield return StartCoroutine(LoadResource());

        //스테이지 이동 시 플레이어 좌표 변경
        int level = SaveData.Instance.LoadingStageLevel;
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            obj.transform.position = SaveData.Instance.GetStartPos(level);
            GameObject.FindGameObjectWithTag("LanternLight")
                .GetComponent<LanternLight>().ReEnable();
        }

        //자원 로딩 완료 시
        loadingPercentage = 100;
        percentage.text = loadingPercentage + "%";
        gaugeSize = (int)STANDARD_SIZE.WIDTH;
        progressBar.sizeDelta = new Vector2(gaugeSize, progressBar.sizeDelta.y);
        asyncLoad.allowSceneActivation = true;
    }

    IEnumerator LoadResource()
    {
        //첫 번째 로딩시에 전역 데이터(사운드, Factory등) 초기화
        if (SaveData.Instance.FirstLoad)
        {
            Instantiate(staticDataLoad);
            while(SettingParser.Instance.LoadingCompletion == false)
            {
                yield return null;
            }
            SaveData.Instance.InitStartPosition();

            SaveData.Instance.FirstLoad = false;
        }

        //이미 데이터가 로딩되어 있는 스테이지의 경우 빠져나오기
        int level = SaveData.Instance.LoadingStageLevel;
        if(SaveData.Instance.GetLoadedStage(level) != 0)
        {
            yield break;
        }

        itemParser.LoadItem(level);
        while(itemParser.LoadingCompletion == false)
        {
            yield return null;
        }
        
        SaveData.Instance.SetLoadedStage(level);
        //다음 맵으로 넘어갈 때 Lantern TriggerStay가 작동안하는 현상 수정할 것
    }
}
