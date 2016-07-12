using UnityEngine;
using System.Collections;

public class MainBody : MonoBehaviour
{
    private bool beShot = false;
    public bool BeShot { get { return beShot; } }

    void OnEnable()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        beShot = false;
    }
    void OnDisable()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "LanternLight")
        {
            //두 오브젝트 사이에 장애물이 존재하는지 검사
            if (Cast(col))
            {
                return;
            }

            beShot = true;
        }
    }

    private bool Cast(Collider col)
    {
        RaycastHit hit;
        Vector3 tarPos = col.transform.position;
        Vector3 objPos = transform.position;

        int layerMask = (1 << 0) | (1 << 11) | (1 << 13);

        bool result = Physics.Raycast(objPos, tarPos - objPos, out hit, (tarPos - objPos).magnitude, layerMask);
        return result;
    }
}
