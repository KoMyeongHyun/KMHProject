using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Title2 : MonoBehaviour
{
    
    [SerializeField]
    private GameObject info;

    public void ClickStart()
    {
        SaveData.Instance.LoadingStageLevel = 1;
        SceneManager.LoadScene("Loading");
    }

    public void ClickInfo()
    {
        StartCoroutine(DisplayInfo());
    }

    public void ClickExit()
    {
        Application.Quit();
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
