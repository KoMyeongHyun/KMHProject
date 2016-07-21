using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Title2 : MonoBehaviour
{
    const int SCREEN_WIDTH = 1280;
    [SerializeField]
    private GameObject loading;
    [SerializeField]
    private GameObject info;

    public void ClickStart()
    {
        StartCoroutine(ChangeSceneAndDisplayLoadingScreen("Game"));
    }

    public void ClickInfo()
    {
        StartCoroutine(DisplayInfo());
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    IEnumerator ChangeSceneAndDisplayLoadingScreen(string sceneName)
    {
        loading.SetActive(true);

        int loadingPercentage = 0;
        Text percentage = loading.transform.GetChild(1).GetComponent<Text>();
        int gaugeSize = 0;
        RectTransform progressBar = loading.transform.GetChild(2).GetComponent<RectTransform>();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.88f)
        {
            loadingPercentage = (int)(asyncLoad.progress * 100);
            percentage.text = loadingPercentage + "%";
            gaugeSize = (int)(SCREEN_WIDTH * loadingPercentage * 0.01);
            progressBar.sizeDelta = new Vector2(gaugeSize, progressBar.sizeDelta.y) ;
            yield return null;
        }

        loadingPercentage = 100;
        percentage.text = loadingPercentage + "%";
        gaugeSize = SCREEN_WIDTH;
        progressBar.sizeDelta = new Vector2(gaugeSize, progressBar.sizeDelta.y);
        //yield return new WaitForSeconds(0.5f);
        asyncLoad.allowSceneActivation = true;
        //loading.SetActive(false);
    }

    IEnumerator DisplayInfo()
    {
        info.SetActive(true);

        while(Input.GetMouseButtonDown(0) == false)
        {
            yield return null;
        }

        info.SetActive(false);
    }
}
