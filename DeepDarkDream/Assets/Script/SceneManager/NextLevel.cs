using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextLevel : MonoBehaviour
{
    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            //화면 전환 코루틴 실행 후에 맵이 변하도록 할 것
            SceneManager.LoadScene("Game2");
        }
    }
}
