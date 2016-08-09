using UnityEngine;
using System.Collections;

public enum INPUT_KIND
{ DROPPED_ITEM, INTERACT, COUNT }

public class InputManager2 : MonoBehaviour
{
    private static InputManager2 instance = null;
    public static InputManager2 Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("InputManager2");
                obj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
                instance = obj.AddComponent<InputManager2>();
            }
            return instance;
        }
    }

    private delegate bool ClickEvent();
    private ClickEvent[] clickEvent;

    private DragRigidbodyUse dragUse;
    private bool inProgress;

    private void Awake()
    {
        clickEvent = new ClickEvent[(int)INPUT_KIND.COUNT];
        clickEvent[(int)INPUT_KIND.DROPPED_ITEM] = ClickForItemPickUp;
        clickEvent[(int)INPUT_KIND.INTERACT] = ClickForInteract;

        dragUse = GameObject.FindGameObjectWithTag("Player").GetComponent<DragRigidbodyUse>();
        inProgress = false;
    }

    public bool MouseButtonDown(INPUT_KIND _kind)
    {
        //UI Layer를 나누고 현재 Layer에 맞는 입력 처리만 하도록 한다.

        //무엇이 먼저 호출 되든지 상관 없어야 한다.
        //문열기나 오브젝트를 들었다 놓는 것이 우선순위가 더 높다
        int kind = (int)_kind;
        bool click = clickEvent[kind]();
        
        return click;
    }

    private bool ClickForInteract()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool ClickForItemPickUp()
    {
        if(inProgress || dragUse.IsObjectHeld)
        {
            return false;
        }
        else if(Input.GetMouseButtonDown(0))
        {
            inProgress = true;
            StartCoroutine(ClickProgress());

            //Dragobject에 Ray가 닿을 경우 false
            RaycastHit hit;
            bool checkRay = dragUse.CheckRay(out hit);
            return (!checkRay);
        }

        return false;
    }

    private IEnumerator ClickProgress()
    {
        while(Input.GetMouseButtonUp(0) == false)
        {
            yield return null;
        }

        inProgress = false;
    }
}
