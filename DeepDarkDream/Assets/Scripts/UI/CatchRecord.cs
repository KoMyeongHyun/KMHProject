using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class CatchRecord : MonoBehaviour
{
    private string[] divisionContent;
    private int curPage;
    private Text curContent;

    public void SetRecord(Item _item)
    {
        Record record = _item as Record;
        string content = record.CONTENT.ToString();
        curContent = transform.GetChild(0).GetComponent<Text>();

        //컨텐츠 분할작업 후 첫번째 페이지를 가리키도록 할 것
        divisionContent = Regex.Split(content, "-next-");
        curPage = 0;
        curContent.text = divisionContent[curPage];

        StartCoroutine(StartContent());
    }
    
    private IEnumerator StartContent()
    {
        while(Input.GetMouseButtonUp(0) == false)
        {
            yield return null;
        }

        while(true)
        {
            yield return null;

            if (Input.GetMouseButtonUp(0))
            {
                ++curPage;
                if (curPage >= divisionContent.Length)
                {
                    break;
                }
                curContent.text = divisionContent[curPage];
            }
            else if (Input.GetMouseButtonUp(1))
            {
                --curPage;
                if (curPage < 0)
                {
                    curPage = 0;
                }
                curContent.text = divisionContent[curPage];
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                break;
            }
        }

        InputManager2.Instance.SwitchFlag(UI_KIND.CATCH_INFO);

        gameObject.SetActive(false);
    }
}
