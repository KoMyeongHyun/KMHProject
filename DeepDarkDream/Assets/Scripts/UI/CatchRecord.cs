using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class CatchRecord : MonoBehaviour
{
    //private bool activity;
    private string[] divisionContent;
    private int curPage;
    private Text curContent;
    private bool startContent;

    public void SetRecord(Item _item)
    {
        Record record = _item as Record;
        string content = record.CONTENT.ToString();
        curContent = transform.GetChild(0).GetComponent<Text>();

        //컨텐츠 분할작업 후 첫번째 페이지를 가리키도록 할 것
        divisionContent = Regex.Split(content, "-next-");
        curPage = 0;
        curContent.text = divisionContent[curPage];

        startContent = false;
        StartCoroutine(StartContent());
    }

    //시작 후에 일단 마우스가 한 번 때어질 때까지 동작 못하도록 설정
    private IEnumerator StartContent()
    {
        while(Input.GetMouseButtonUp(0) == false)
        {
            yield return null;
        }
        startContent = true;
    }

    void Update()
    {
        if(startContent == false)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ++curPage;
            if(curPage >= divisionContent.Length)
            {
                gameObject.SetActive(false);
                return;
            }
            curContent.text = divisionContent[curPage];
        }
        else if(Input.GetMouseButtonUp(1))
        {
            --curPage;
            if (curPage < 0)
            {
                curPage = 0;
                return;
            }
            curContent.text = divisionContent[curPage];
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            return;
        }
    }


    //if (activity == true)
    //{
    //    gameObject.SetActive(false);
    //    activity = false;
    //    return;
    //}
    //activity = true;
}
