using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

public enum UI_KIND
{
    NONE = 0x1 << 0,
    CATCH_INFO = 0x1 << 1,
    INVENTORY = 0x1 << 2,
    PAUSE = 0x1 << 3
}

public enum UI_NONE_INPUT_KIND
{
    DROPPED_ITEM,
    INTERACT,
    RUNE_STONE,
    COUNT
}

public class InputManager2 : MonoBehaviour
{
    private static InputManager2 instance = null;
    public static InputManager2 Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("InputManager2");
                obj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
                instance = obj.AddComponent<InputManager2>();
            }
            return instance;
        }
    }
    
    private int flag;
    public int Flag
    {
        set { flag = value; }
        get { return flag; }
    }
    public void SwitchFlag(UI_KIND _UIKind)
    {
        flag ^= (int)_UIKind;
        if(flag == 0)
        {
            flag = (int)UI_KIND.NONE;
        }
    }

    //클래스명, 변수명 바꿀 것
    private InputManager assistance;
    private FirstPersonController player;
    private GameObject inven;
    private Vector3 invenPos;
    private Vector3 hideInvenPos;

    private delegate bool ClickEvent();
    private ClickEvent[] clickEvent;

    private DragRigidbodyUse dragUse;
    private bool inProgress;

    private void Awake()
    {
        flag = (int)UI_KIND.NONE;

        assistance = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InputManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        inven = GameObject.FindGameObjectWithTag("Inventory");
        invenPos = new Vector3(Screen.width * 0.65f, Screen.height * 0.5f);
        hideInvenPos = new Vector3(5000.0f, 5000.0f);
        inven.transform.position = hideInvenPos;

        HideCursor();

        clickEvent = new ClickEvent[(int)UI_NONE_INPUT_KIND.COUNT];
        clickEvent[(int)UI_NONE_INPUT_KIND.DROPPED_ITEM] = ClickForItemPickUp;
        clickEvent[(int)UI_NONE_INPUT_KIND.INTERACT] = ClickForInteract;
        clickEvent[(int)UI_NONE_INPUT_KIND.RUNE_STONE] = ClickForItemPickUp;

        dragUse = GameObject.FindGameObjectWithTag("Player").GetComponent<DragRigidbodyUse>();
        inProgress = false;
    }
    
    public IEnumerator InputUserInterface()
    {
        do
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Esc 클릭에 대한 처리 추가
                //인터페이스 켜진 순서대로 닫아 주어야 하는지
                //기존 정렬 순서대로 닫아줄지
            }

            if ( Input.GetKeyDown(KeyCode.P) )
            {
                //일시정지
                //각종 예외 처리 필요, 타격 도중일 때 등
                if(Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                else
                {
                    Time.timeScale = 0;
                }
            }

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

            //제한 조건은 요구에 따라 유동적으로 변화시킬 것
            if ((flag & (int)UI_KIND.CATCH_INFO) != 0)
            {
                continue;
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log(inven.transform.position);
                SwitchFlag(UI_KIND.INVENTORY);

                if ( (flag & (int)UI_KIND.INVENTORY) != 0 )
                {
                    ShowCursor();
                    player.setStopBehavior(true);
                    inven.transform.position = invenPos;
                }
                else
                {
                    HideCursor();
                    player.setStopBehavior(false);
                    inven.transform.position = hideInvenPos;
                }
            }

        } while (true);
    }

    public bool MouseButtonDown(UI_NONE_INPUT_KIND _kind)
    {
        //무엇이 먼저 호출 되든지 상관 없어야 한다.
        //문열기나 오브젝트를 들었다 놓는 것이 우선순위가 더 높다
        if ( (flag != (int)UI_KIND.NONE) )
        {
            return false;
        }

        //UI NONE 마우스 클릭 상황
        int kind = (int)_kind;
        return clickEvent[kind]();
    }

    private bool ClickForInteract()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool ClickForRuneStone()
    {
        //별도의 예외처리가 필요하면 이 함수 사용할 것
        if (dragUse.IsObjectHeld)
        {
            return false;
        }

        return Input.GetMouseButtonDown(0);
    }

    private bool ClickForItemPickUp()
    {
        if(inProgress || dragUse.IsObjectHeld)
        {
            return false;
        }
        else if(Input.GetMouseButtonDown(0))
        {
            inProgress = true;
            StartCoroutine(ClickProgress());

            //Dragobject에 Ray가 닿을 경우 false
            RaycastHit hit;
            bool checkRay = dragUse.CheckRay(out hit);
            return (!checkRay);
        }

        return false;
    }

    private IEnumerator ClickProgress()
    {
        while(Input.GetMouseButtonUp(0) == false)
        {
            yield return null;
        }

        inProgress = false;
    }

    private void ShowCursor()
    {
        Texture2D defaultCursor = assistance.DefaultCursor;
        Vector2 defaultHotSpot = assistance.DefaultHotSpot;

        Vector2 hotspot = new Vector2(defaultCursor.width * defaultHotSpot.x, defaultCursor.height * defaultHotSpot.y);
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
