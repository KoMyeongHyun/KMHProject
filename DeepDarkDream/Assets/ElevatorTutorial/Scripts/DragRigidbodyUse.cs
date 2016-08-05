/*
DragRigidbodyUse.cs ver. 11.1.16 - wirted by ThunderWire Games * Script for Drag & Drop & Throw Objects & Draggable Door & PickupObjects
*/

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GrabObjectClass
{
    public bool m_FreezeRotation;
    public float m_PickupRange = 3f;
    public float m_ThrowStrength = 50f;
    public float m_distance = 1f;
    public float m_maxDistanceGrab = 4f;
}

[System.Serializable]
public class TagsClass
{
    public string m_InteractTag = "Interact";
    public string m_InteractItemsTag = "InteractItem";
    public string m_DoorsTag = "Door";
    public string m_ChestTag = "Chest";
}

public class DragRigidbodyUse : MonoBehaviour
{
	public GameObject playerCam;
	
	public string GrabButton = "Grab";
	public string ThrowButton = "Throw";
	public string UseButton = "Use";
	public GrabObjectClass ObjectGrab = new GrabObjectClass();
	public TagsClass Tags = new TagsClass();
	
	private float PickupRange = 3f;
	private float ThrowStrength = 50f;
	private float distance = 3f;
	private float maxDistanceGrab = 4f;
	
	private Ray playerAim;
	private GameObject objectHeld;
	private bool isObjectHeld;
    private bool saveFreezeState;
    private Transform saveOjbectHeld;
	
	void Start ()
    {
        objectHeld = null;
        isObjectHeld = false;
        saveFreezeState = false;

        GameObject obj = new GameObject("Interact Rot Dummy");
        obj.transform.SetParent(playerCam.transform, false);
        saveOjbectHeld = obj.transform;
    }
	
    void Update()
    {
        //스테이지 넘어가면 잡고 있는 오브젝트 놓기
        //타격 받으면 오브젝트 놓기?
        if (Input.GetButtonDown(GrabButton))
        {
            if (isObjectHeld == false)
            {
                //오브젝트 집기 시도
                TryPickObject();
            }
            else
            {
                DropObject();
            }
        }
    }

    private void TryDoorAction()
    {
        Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if(Physics.Raycast(playerAim, out hit, PickupRange))
        {
            if(hit.collider.tag == Tags.m_DoorsTag)
            {
                //조건 걸고 부모가 아니라면 자기자신으로
                hit.collider.GetComponentInParent<Door2>().SendMessage("ChangeDoorState");
            }
            else if(hit.collider.tag == Tags.m_ChestTag)
            {
                hit.collider.GetComponent<Chest>().SendMessage("OpenChest");
            }
        }
    }

	private void TryPickObject()
    {
		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		
		if (Physics.Raycast (playerAim, out hit, PickupRange))
        {
			objectHeld = hit.collider.gameObject;
			if(hit.collider.tag == Tags.m_InteractTag)
            {
                Rigidbody rig;
                if(objectHeld.GetComponent<Rigidbody>() == null)
                {
                    //부모에 Rigidbody가 붙어있을 경우
                    rig = objectHeld.GetComponentInParent<Rigidbody>();
                    objectHeld = objectHeld.transform.parent.gameObject;
                }
                else
                {
                    rig = objectHeld.GetComponent<Rigidbody>();
                }

                isObjectHeld = true;
                rig.useGravity = false;
                saveFreezeState = rig.freezeRotation;
                if (ObjectGrab.m_FreezeRotation)
                {
                    rig.freezeRotation = true;
				}
				if (ObjectGrab.m_FreezeRotation == false)
                {
                    rig.freezeRotation = false;
				}
                saveOjbectHeld.rotation = objectHeld.transform.rotation;
                /**/
                PickupRange = ObjectGrab.m_PickupRange; 
				ThrowStrength = ObjectGrab.m_ThrowStrength;
				distance = ObjectGrab.m_distance;
				maxDistanceGrab = ObjectGrab.m_maxDistanceGrab;

                StartCoroutine(HoldObject());
                return;
            }
		}
        TryDoorAction();
    }

	private IEnumerator HoldObject()
    {
        while (isObjectHeld)
        {
            Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
            Vector3 currPos = objectHeld.transform.position;
            
            objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 20.0f;
            objectHeld.GetComponent<Rigidbody>().MoveRotation(saveOjbectHeld.rotation);

            if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
            {
                DropObject();
            }

            yield return new WaitForFixedUpdate();
        }
	}
	
    private void DropObject()
    {
		isObjectHeld = false;
		objectHeld.GetComponent<Rigidbody>().useGravity = true;
        objectHeld.GetComponent<Rigidbody>().freezeRotation = saveFreezeState;
        objectHeld = null;
    }
	
    private void ThrowObject()
    {
        objectHeld.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * ThrowStrength, ForceMode.Impulse);
		//objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		objectHeld = null;
    }
	
    private void Use()
    {
        //Every script attached to the PickupObject that has a UseObject function will be called.
        objectHeld.SendMessage ("UseObject",SendMessageOptions.DontRequireReceiver); 
		objectHeld = null;
    }
}
