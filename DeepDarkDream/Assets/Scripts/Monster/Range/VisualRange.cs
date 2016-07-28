using UnityEngine;
using System.Collections;

public class VisualRange : MonoBehaviour
{
    private bool visualTarget = false;
    public bool VisualTarget { get { return visualTarget; } }

    void OnEnable()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        visualTarget = false;
    }
    void OnDisable()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    //Rigidbody 있는 오브젝트와 충돌해야 감지
    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            //두 오브젝트 사이에 장애물이 존재하는지 검사
            if (Cast(col))
            {
                return;
            }

            visualTarget = true;
        }
    }

    private bool Cast(Collider col)
    {
        RaycastHit hit;
        Vector3 tarPos = col.transform.position;
        Vector3 objPos = transform.position;

        //180도 이상(뒤에 존재)이라면 전부 true 리턴
        float permissionVal = Vector3.Dot(transform.forward, tarPos-objPos);
        if (permissionVal <= 0.0f)
        {
            return true;
        }

        //Debug.DrawRay(transform.position, col.transform.position - transform.position, Color.gray);
        //int layerMask = (1 << 8) | (1 << 10) | (1 << 12); //8 player 10 collisionExecption 12 item
        //layerMask = ~layerMask;

        //Rigidbody 있는 것 중에서 제외할 대상 : 인터렉트 오브젝트(13), 문 오브젝트(11)
        //Rigidbody 없는 것 중에서 제외할 대상 : Default 일반 벽 같은 것(0)
        int layerMask = (1 << 0) | (1 << 11) | (1 << 13);
        
        bool result = Physics.Raycast(objPos, tarPos - objPos, out hit, (tarPos - objPos).magnitude, layerMask);
        return result;
    }
}
