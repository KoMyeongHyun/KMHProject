using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject loadingText;
    [SerializeField]
    private GameObject percentageText;
    [SerializeField]
    private GameObject progressBar;
    [SerializeField]
    private GameObject gameRole;

    private int buttonWidth = 200;
    private int buttonHeight = 50;
    private int spacing = 25;

    private bool progress;
    private int loadingPercentage;

	// Use this for initialization
	void Start ()
    {
        background.SetActive(false);
        loadingText.SetActive(false);
        percentageText.SetActive(false);
        progressBar.SetActive(false);
        gameRole.SetActive(false);
        progress = false;
        loadingPercentage = 0;
    }

    void OnGUI()
    {
        if (progress)
            return;

        GUILayout.BeginArea(new Rect(Screen.width / 2 - buttonWidth / 2, Screen.height / 2 - 100, buttonWidth, 400));

        //게임 시작
        
        if (GUILayout.Button("Game Start", GUILayout.Height(buttonHeight)))
        {
            StartCoroutine(ChangeSceneAndDisplayLoadingScreen("Game"));
        }
        
        GUILayout.Space(spacing);

        //게임 방법  
        if (GUILayout.Button("Game Info", GUILayout.Height(buttonHeight)))
        {
            DisplayGameRoleScreen();
        }
        
        GUILayout.Space(spacing);

        //게임 종료
        if (GUILayout.Button("Exit", GUILayout.Height(buttonHeight)))
        {
            Application.Quit();
        }
        
        GUILayout.EndArea();
    }

    IEnumerator ChangeSceneAndDisplayLoadingScreen(string sceneName)
    {
        if (progress)
            yield break;

        progress = true;

        background.SetActive(true);
        loadingText.SetActive(true);
        percentageText.SetActive(true);
        progressBar.SetActive(true);

        Transform pb = progressBar.transform;
        pb.localScale = new Vector3(loadingPercentage, pb.localScale.y, pb.localScale.z);

        percentageText.GetComponent<GUIText>().text = loadingPercentage + "%";

        AsyncOperation asyncLoad = Application.LoadLevelAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        //while (asyncLoad.isDone == false)
        while (asyncLoad.progress < 0.88f)
        {
            loadingPercentage = (int)(asyncLoad.progress * 100);
            percentageText.GetComponent<GUIText>().text = loadingPercentage + "%";
            pb.localScale = new Vector3(asyncLoad.progress, pb.localScale.y, pb.localScale.z);
            yield return null;
        }

        loadingPercentage = 100;
        percentageText.GetComponent<GUIText>().text = loadingPercentage + "%";
        pb.localScale = new Vector3(1.0f, pb.localScale.y, pb.localScale.z);

        yield return new WaitForSeconds(1.0f);
        asyncLoad.allowSceneActivation = true;
        
        //progress = false;

    }

    void DisplayGameRoleScreen()
    {
        progress = true;
        gameRole.SetActive(true);
        StartCoroutine(ProcessGameRole());
    }
    IEnumerator ProcessGameRole()
    {
        while(true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                progress = false;
                gameRole.SetActive(false);
                break;
            }
            yield return null;
        }
    }
}
