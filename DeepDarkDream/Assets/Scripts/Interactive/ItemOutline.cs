using UnityEngine;
using System.Collections;

public class ItemOutline : MonoBehaviour
{
    void Start()
    {
        SetOutline(0);
    }

    public void SetOutline(float _size)
    {
        MeshRenderer renderer = transform.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            for(int i = 0; i < transform.childCount; ++i)
            {
                renderer = transform.GetChild(i).GetComponent<MeshRenderer>();
                renderer.material.SetFloat("_Outline", _size);
            }
        }
        else
        {
            renderer.material.SetFloat("_Outline", _size);
        }
    }
}
