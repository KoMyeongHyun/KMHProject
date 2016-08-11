using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    Texture2D defaultCursor;
    public Texture2D DefaultCursor { get { return defaultCursor; } }
    [SerializeField]
    Vector2 defaultHotSpot;
    public Vector2 DefaultHotSpot { get { return defaultHotSpot; } }

    [SerializeField]
    private GameObject catchInfo;
    [SerializeField]
    private GameObject catchRecord;

    void Start()
    {
        StartCoroutine(InputManager2.Instance.InputUserInterface());

        DontDestroyOnLoad(this);
    }

    public void ShowCatchItem(Item _item)
    {
        InputManager2.Instance.SwitchFlag(UI_KIND.CATCH_INFO);

        switch(_item.TYPE)
        {
            case ItemType.RECORD:
                ShowCatchRecord(_item);
                break;
            default:
                ShowCatchInfo(_item);
                break;
        }
    }

    private void ShowCatchInfo(Item _item)
    {
        catchInfo.SetActive(true);
        catchInfo.GetComponent<CatchInfo>().SetInfo(_item);
    }

    private void ShowCatchRecord(Item _item)
    {
        catchRecord.gameObject.SetActive(true);
        catchRecord.GetComponent<CatchRecord>().SetRecord(_item);
    }
}