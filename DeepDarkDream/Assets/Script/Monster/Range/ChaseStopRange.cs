using UnityEngine;
using System.Collections;

public class ChaseStopRange : MonoBehaviour
{
    private bool chaseStop = false;
    public bool ChaseStop { get{ return chaseStop; } }
    private bool inRange = false;
    public bool InRange { get { return inRange; } }

    void OnEnable()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        chaseStop = false;
        inRange = false;
    }
    void OnDisable()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    //스크립트가 비활성화 상태여도 콜라이더가 충돌 시 아래 함수 호출
    //그럴경우, 콜라이더의 충돌 체크 상태는 지속적으로 유지된다.
    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            inRange = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            chaseStop = true;
            inRange = false;
        }
    }
}
