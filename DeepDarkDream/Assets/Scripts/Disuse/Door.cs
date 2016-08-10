using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField] private float openAngle;
    [SerializeField] private float closeAngle;
    [SerializeField] private bool doorLock;
    [SerializeField] private string keyName;
    [SerializeField] private bool openedDoor;

    //private InputManager inputManager;
    private Transform cameraTrans;
    private Inventory inven;
    private float startAngle;
    private float targetAngle;
    private bool processOfRotation;
    private float direction;

	// Use this for initialization
	void Awake ()
    {
        openAngle = (openAngle % 360) + 360;
        openAngle = openAngle >= 360 ? (openAngle % 360) : openAngle;
        closeAngle = (closeAngle % 360) + 360;
        closeAngle = closeAngle >= 360 ? (closeAngle % 360) : closeAngle;
        
        float a = openedDoor ? openAngle : closeAngle;
        transform.localRotation = Quaternion.Euler(0.0f, a, 0.0f);

        //inputManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InputManager>();
        cameraTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        inven = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        startAngle = 0.0f;
        targetAngle = 0.0f;
        processOfRotation = false;
        direction = (openAngle > 180.0f) ? 1.0f : -1.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    private IEnumerator RotateDoor()
    {
        processOfRotation = true;
        Quaternion start = Quaternion.Euler(0.0f, startAngle, 0.0f);
        Quaternion target = Quaternion.Euler(0.0f, targetAngle, 0.0f);
        Quaternion result = Quaternion.Lerp(start, target, Time.deltaTime);

        float a = transform.localEulerAngles.y - result.eulerAngles.y;
        a = Mathf.Abs(a);
        a = (360.0f - a) < a ? (360.0f - a) : a;
        Vector3 eulerA = new Vector3(0.0f, a, 0.0f);
        float dir = 0.0f;

        while (Mathf.Abs((transform.localEulerAngles.y % 360.0f) - targetAngle) > 3.0f)
        {
            dir = openedDoor ? -direction : direction;
            transform.Rotate(eulerA * dir);
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);

        processOfRotation = false;
        yield return null;
    }

    public void OnTriggerEnter(Collider col)
    {
    }

    public void OnTriggerStay(Collider col)
    {
        //조건 만족 시 커서 모양을 바꿔준다. 불만족시 없애기
        if(col.tag == "Player")
        {
            //플레이어가 문을 가리킬 경우
            if (Cast(col))
            {
                //inputManager.ChangeCursor();

                if (Input.GetMouseButtonDown(0))
                {
                    if (processOfRotation)
                    { return; }

                    if (doorLock)
                    {
                        //인벤토리 서치 후 결과에 따라 코루틴보내서 작업할거 진행 후 락 풀어주기
                        //열쇠 없으면 그냥 리턴
                        if (inven.SearchItem(keyName) == false)
                            return;

                    }

                    openedDoor = !openedDoor;
                    if (openedDoor)
                    {
                        startAngle = closeAngle;
                        targetAngle = openAngle;
                    }
                    else
                    {
                        startAngle = openAngle;
                        targetAngle = closeAngle;
                    }

                    //if(processOfRotation == false)
                    {
                        StartCoroutine(RotateDoor());
                    }
                }
            }
            else
            {
                //inputManager.HideCursor();
            }

        }
    }

    public void OnTriggerExit(Collider col)
    {
        //커서 없애기
        //inputManager.HideCursor();
    }

    private bool Cast(Collider col)
    {
        RaycastHit hit;
        //float maxDistance = ((player.position - transform.position)*2).magnitude;
        float maxDistance = 10.0f;
        int layerMask = 1 << 11;

        //플레이어가 레이를 쏴서 도어 레이어에 맞으면
        return Physics.Raycast(cameraTrans.position, cameraTrans.forward, out hit, maxDistance, layerMask);
    }
}
