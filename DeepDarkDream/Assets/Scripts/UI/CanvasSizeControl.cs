using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Screen.SetResolution(1280, 720, true);
public enum STANDARD_SIZE
{
    WIDTH = 1280,
    HEIGHT = 720
}

public class CanvasSizeControl : MonoBehaviour
{
    private Vector2 ratio;
    public Vector2 Ratio
    {
        get
        {
            return ratio;
        }
    }

    void Awake()
    {
        CanvasScaler canvasScaler = gameObject.GetComponent<CanvasScaler>();
        canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);

        //모든 자식들의 스케일을 조정한다
        float x = Screen.width / (float)STANDARD_SIZE.WIDTH;
        float y = Screen.height / (float)STANDARD_SIZE.HEIGHT;
        ratio = new Vector2(x, y);
        
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).localScale = ratio;
        }
    }
}
