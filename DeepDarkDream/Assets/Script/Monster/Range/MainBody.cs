using UnityEngine;
using System.Collections;

public class MainBody : MonoBehaviour
{
    private bool beShot = false;
    public bool BeShot { get { return beShot; } }

    void OnEnable()
    {
        beShot = false;
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "LanternLight")
        {
            beShot = true;
        }
    }

    private bool Cast(GameObject obj, Collider col)
    {
        RaycastHit hit;

        Vector3 tarPos = col.transform.position;
        Vector3 objPos = obj.transform.position;

        Debug.DrawRay(objPos, tarPos - objPos, Color.gray);

        int layerMask = (1 << 8) | (1 << 10);
        layerMask = ~layerMask;

        bool result = Physics.Raycast(objPos, tarPos - objPos, out hit, (tarPos - objPos).magnitude, layerMask);
        return result;
    }
}
