using UnityEngine;
using System.Collections;

public class CamearManager : MonoBehaviour
{

    public Transform target;
    private float distance;
    private float xSpeed, ySpeed;
    private float yMinLimit, yMaxLimit;
    private float x, y;

    // Use this for initialization
    void Awake()
    {
        distance = 3.0f;
        xSpeed = 250.0f;
        ySpeed = 120.0f;
        yMinLimit = 270.0f;   //5
        yMaxLimit = 360.0f;  //80

        //카메라의 오일러 각 대입
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;   //x축 회전에 대해서

        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) return;

        bool check = false;
        if (cast())
        {
            distance -= 0.1f;
        }
        else
        {
            check = true;
            distance += 0.1f;
            if (distance >= 3.0f)
                distance = 3.0f;
        }
        //distance = 0.0f;

        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y += Input.GetAxis("Mouse Y") * ySpeed * 0.02f;     //Mouse Vertical Move에 대한 회전 처리

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(-y, x, .0f); //오일러 각을 쿼터니언으로 변환
        Vector3 position = rotation * new Vector3(.0f, .0f, -distance) + target.position + new Vector3(.0f, 1.0f, .0f);

        transform.rotation = rotation;
        transform.position = position;


        if (cast() == true && check == true)
        {
            distance -= 0.1f;
            Vector3 ss = rotation * new Vector3(.0f, .0f, -distance) + target.position + new Vector3(.0f, 1.0f, .0f);
            transform.position = ss;
        }

    }
    public float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle = 360;

        return Mathf.Clamp(angle, min, max);
    }

    //카메라가 벽 뒤에 있을 때 처리
    public bool cast()
    {
        RaycastHit hit;
        /*  Vector3 aaa = transform.position - target.position;
          //aaa.Normalize();

          Debug.DrawRay(transform.position, -aaa, Color.green);

          if( Physics.Raycast(transform.position, -aaa, out hit, 20f) )
          {
              Debug.Log("gggg");
              transform.position = transform.position + aaa * (hit.distance - 1);
          }
          else
          {
              Debug.Log("nonon");
              Vector3 defaultVec = transform.position + aaa * 20.0f;
              transform.position = defaultVec;
          }*/

        Vector3 tar = target.position + new Vector3(.0f, 1.0f, .0f);
        //Vector3 result = Vector3.zero;

        Debug.DrawRay(transform.position, tar - transform.position, Color.gray);

        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        if (Physics.Raycast(transform.position, tar - transform.position, out hit, (tar - transform.position).magnitude, layerMask))
        {
            
     //       distance -= 0.1f;
            return true;
            //Debug.Log(hit.distance);
            //result = hit.normal * 2f;
            //transform.position = transform.position + (tar + transform.position) * (hit.distance - 1);
            //targetPosition = (hit.point - transform.position)  0.8f + playerTransform.position;
            /* Note that I move the camera to 80 % of the distance to the point where an obstruction has been found to help keep the sides of the frustrum from still clipping through the wall */
        }
        else
        {
    //        if (distance < 3.0f)
     //           distance += 0.1f;
            //더한 후 true가 되면 더하지 말 것
    //        else
     //           distance = 3.0f;


            return false;
            //distance = 3.0f;
            //targetPosition = desiredPosition;
        }


    }
}