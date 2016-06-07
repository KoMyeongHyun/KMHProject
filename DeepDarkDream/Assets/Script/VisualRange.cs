using UnityEngine;
using System.Collections;

public class VisualRange : MonoBehaviour
{
    private bool visualTarget = false;
    public bool VisualTarget { get { return visualTarget; } }

    void OnEnable()
    {
        visualTarget = false;
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (Cast(col) == true)
                return;

            visualTarget = true;
        }
    }

    private bool Cast(Collider col)
    {
        RaycastHit hit;
        Vector3 tarPos = col.transform.position;
        Vector3 objPos = transform.position;

        //180도 이상이라면 전부 리턴
        float permissionVal = Vector3.Dot(transform.forward, tarPos-objPos);
        if (permissionVal <= 0.0f)
            return true;

        //Debug.DrawRay(transform.position, col.transform.position - transform.position, Color.gray);

        int layerMask = (1 << 8) | (1 << 10) | (1 << 12);
        layerMask = ~layerMask;

        bool result = Physics.Raycast(objPos, tarPos - objPos, out hit, (tarPos - objPos).magnitude, layerMask);
        return result;
    }
}
