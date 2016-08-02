using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    Texture2D defaultCursor;
    [SerializeField]
    Vector2 DefaultHotSpot;
    [SerializeField]
    Texture2D handCursor;
    [SerializeField]
    Vector2 handHotSpot;

    [SerializeField]
    private GameObject catchInfo;
    public bool ActiveCatchInfo
    {
        get
        {
            return catchInfo.activeSelf;
        }
    }
    [SerializeField]
    private GameObject catchRecord;
    public bool ActiveCatchRecord
    {
        get
        {
            return catchRecord.activeSelf;
        }
    }

    private GameObject inven;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    private Vector3 invenPos;
    private Vector3 hideInvenPos;
    private bool openInven;
    public bool OpenInven { get { return openInven; } }

    // Use this for initialization
    void Start()
    {
        ShowCursor();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inven = GameObject.FindGameObjectWithTag("Inventory");
        player = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

        invenPos = new Vector3(Screen.width * 0.65f
                , Screen.height * 0.5f);
        hideInvenPos = new Vector3(5000.0f, 5000.0f);
        inven.transform.position = hideInvenPos;
        openInven = false;
        
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        //추후 수정할 것
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowCursor();
            SaveData.Instance.LoadingStageLevel = 0;

            //player, canvas 존재하면 지우기
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            Vector3 existingPos = obj.transform.position;
            Destroy(obj);

            obj = GameObject.FindGameObjectWithTag("Canvas");
            Destroy(obj);

            //게임 종료 후 에러 방지
            obj = new GameObject("AudioListener");
            obj.AddComponent<AudioListener>();
            obj.AddComponent<Camera>().transform.position = existingPos;
            NotificationCenter.DefaultCenter.RemoveAllObserver();

            SceneManager.LoadScene("Title");
        }

        if (catchInfo.activeSelf || catchRecord.activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(inven.transform.position);
            openInven = !openInven;

            if (openInven)
            {
                ShowCursor();

                inven.transform.position = invenPos;
                player.setStopBehavior(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                inven.transform.position = hideInvenPos;
                player.setStopBehavior(false);
            }
        }

    }

    private void ShowCursor()
    {
        Vector2 hotspot = new Vector2(defaultCursor.width * DefaultHotSpot.x, defaultCursor.height * DefaultHotSpot.y);
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ChangeCursor()
    {
        //Vector2 hotspot = new Vector2(handCursor.width * handHotSpot.x, handCursor.height * handHotSpot.y);
        //Cursor.SetCursor(handCursor, hotspot, CursorMode.Auto);
        //if (Cursor.visible == true)
        //{
        //    return;
        //}

        //        Cursor.visible = true;
    }
    public void HideCursor()
    {
        //if(Cursor.visible == false)
        //{
        //    return;
        //}

        //        Cursor.visible = false;
    }

    public void ShowCatchItem(Item _item)
    {
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