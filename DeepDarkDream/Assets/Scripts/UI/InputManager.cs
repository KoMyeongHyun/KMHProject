using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InputManager : MonoBehaviour {

    [SerializeField] Texture2D defaultCursor;
    [SerializeField] Vector2 DefaultHotSpot;
    [SerializeField] Texture2D handCursor;
    [SerializeField] Vector2 handHotSpot;

    private GameObject inven;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    private Vector3 invenPos;
    private Vector3 hideInvenPos;
    private bool openInven;
    public bool OpenInven { get { return openInven; } }

	// Use this for initialization
	void Start ()
    {
        ShowCursor();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inven = GameObject.FindGameObjectWithTag("Inventory");
        player = GameObject.FindGameObjectWithTag("Player")
            .GetComponent< UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        invenPos = new Vector3(Screen.width * 0.8f, Screen.height * 0, 5f);
        hideInvenPos = new Vector3(5000.0f, 5000.0f);
        inven.transform.position = hideInvenPos;
        openInven = false;
        //inven.SetActive(openInven);

        DontDestroyOnLoad(this);
    }
	
	void Update ()
    {
        //추후 수정할 것
        if(Input.GetKeyDown(KeyCode.Tab))
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

            SceneManager.LoadScene("Title");
        }

        if (inven.GetComponent<Inventory>().catchInfo.activeSelf == true
             || inven.GetComponent<Inventory>().CatchRecord.isActiveAndEnabled == true)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            invenPos = new Vector3(Screen.width * 0.75f, Screen.height * 0.5f);
            Debug.Log(inven.transform.position);
            openInven = !openInven;

            if(openInven)
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
}
