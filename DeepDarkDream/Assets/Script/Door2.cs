using UnityEngine;
using System.Collections;

public class Door2 : MonoBehaviour
{
    public enum DoorState
    {
        OPEN,
        CLOSE,
        COUNT
    };
    private DoorState doorState;
    private float[] destAngle;
    private bool playerCollision;
    //각도에 따른 문 열리는 방향 설정
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    private float dir;
    private SoundController soundController;
    private Inventory inven;

    [SerializeField] private float openAngle;
    [SerializeField] private float closeAngle;
    [SerializeField] private float direction;
    [SerializeField] private bool doorLock;
    [SerializeField] private string keyName;

    // Use this for initialization
    void Start()
    {
        doorState = DoorState.CLOSE;
        destAngle = new float[(int)DoorState.COUNT];
        //destAngle[(int)DoorState.CLOSE] = transform.rotation.eulerAngles.y + gameObject.GetComponent<HingeJoint>().limits.min;
        //destAngle[(int)DoorState.OPEN] = transform.rotation.eulerAngles.y - gameObject.GetComponent<HingeJoint>().limits.max;
        destAngle[(int)DoorState.CLOSE] = closeAngle;
        destAngle[(int)DoorState.OPEN] = openAngle;
        playerCollision = false;

        player = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

        dir = 3.0f * direction;
        soundController = new SoundController(this.gameObject);
        inven = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<Rigidbody>().isKinematic == true)
        {
            return;
        }

        //방향을 구해야 한다.
        gameObject.GetComponent<Rigidbody>().velocity = transform.right.normalized * dir;


        //기본적으로 현재 상태 쪽 가면 닫혀서 고정되도록
        float interval = destAngle[(int)doorState] - transform.rotation.eulerAngles.y;
        interval = Mathf.Abs(interval);
        interval = (360.0f - interval) < interval ? (360.0f - interval) : interval;
        
        if(interval < 0.1f)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        //추가적으로 플레이어 충돌 시 끝부분이면 고정
        if (playerCollision == false)
        {
            return;
        }

        interval = destAngle[(int)(doorState ^ DoorState.CLOSE)] - transform.rotation.eulerAngles.y;
        interval = Mathf.Abs(interval);
        interval = (360.0f - interval) < interval ? (360.0f - interval) : interval;

        //Vector3 doo = transform.position;
        //doo.y = playerCon.transform.position.y;
        //Vector3 a = transform.position - playerCon.transform.position;
        //Debug.DrawRay(playerCon.transform.position, doo - playerCon.transform.position);
        

        if (interval < 5.0f)
        {
            //플레이어가 다가오는 방향으로 힘을 준다.

            //gameObject.GetComponent<Rigidbody>().velocity = a * 20.0f;
            dir = -dir;
            doorState ^= DoorState.CLOSE;
            playerCollision = false;
            gameObject.GetComponent<Rigidbody>().velocity = transform.right.normalized * dir;

            //플레이어 정지 시킬 것
            //후에 플레이어를 밀어 줘야 한다. 일정시간 밀어주는 코루틴 실행
//            player.SendMessage("StopAndKnockback", transform.GetChild(0));

            //gameObject.GetComponent<Rigidbody>().AddForce(a * 30.0f, ForceMode.Impulse);
            //playerCon.Move(transform.right.normalized * (-dir));
        }
    }

    public void ChangeDoorState()
    {
        //달리기 중일때에는 문을 열지 못하도록한다.
        if (gameObject.GetComponent<Rigidbody>().isKinematic && player.Run)
        {
            return;
        }

        if (doorLock)
        {
            //인벤토리 서치 후 결과에 따라 락 풀어주기
            //열쇠 없으면 그냥 리턴
            if (inven.SearchItem(keyName) == false)
            {
                //철컥 사운드
                soundController.ChangeAndPlay("LockDoor");

            }
            else
            {
                //키 사용 사운드
                doorLock = false;
                soundController.ChangeAndPlay("UsedKey");
            }

            return;
        }
        //문 여는 사운드
        soundController.ChangeAndPlay("OpenDoor");

        dir = -dir;
        doorState ^= DoorState.CLOSE;
        playerCollision = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerCollision = true;
        }
    }
    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerCollision = false;
        }
    }
}
