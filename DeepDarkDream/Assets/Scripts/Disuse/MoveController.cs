using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {

    Animator anim;

    public CharacterController charController;

    public float moveSpeed;
    public float rotSpeed;
    private float moveOn;

    private bool back = false;

    private Vector3 a;
    private Vector3 b;

    bool overlap = false;
    bool overlap2 = false;

    public AudioClip walkSound;
    private SoundController playerSound;
    
    // Use this for initialization
    void Start () {
        anim = this.GetComponent<Animator>();
        charController = this.GetComponent<CharacterController>();

        SoundPool.Instance.AddSoundClip("PlayerStep1", walkSound);
        playerSound = new SoundController(gameObject);
        playerSound.ChangeSound("PlayerStep1");
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        //MoveRot();
        moveMode();
    }

    void Move()
    {
        Transform cameraTransform = Camera.main.transform;
        //로컬에서 월드로 방향을 반환, 길이는 그대로 (카메라 정면 방향을 얻는다)
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        forward.Normalize();

        Vector3 forwardForce = Vector3.zero;
        forwardForce = forward * Input.GetAxis("Vertical") * moveSpeed;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        right.Normalize();

        Vector3 rightForce = Vector3.zero;
        rightForce = right * Input.GetAxis("Horizontal") * moveSpeed;


        Quaternion initRot = transform.rotation;
        bool aa = false;
        bool bb = false;

        if ( Input.GetKeyDown(KeyCode.W) )
        {
            playerSound.PlaySound();
            moveOn = 1.0f;

            if (anim.GetBool("MoveMode") == false)
                anim.SetFloat("Walk", moveOn);
            else if (anim.GetBool("MoveMode") == true)
                anim.SetFloat("Run", moveOn);

            anim.SetBool("Girls", false);
        }
        

        else if (Input.GetKeyUp(KeyCode.W))
        {
           // moveOn = 0.0f;
            if (anim.GetBool("MoveMode") == false)
                anim.SetFloat("Walk", moveOn);
            else if (anim.GetBool("MoveMode") == true)
                anim.SetFloat("Run", moveOn);

            a = Vector3.zero;
            //aa = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerSound.PlaySound();
            moveOn = 1.0f;
            if (anim.GetBool("MoveMode") == false)
                anim.SetFloat("Walk", moveOn);
            else if (anim.GetBool("MoveMode") == true)
                anim.SetFloat("Run", moveOn);

            anim.SetBool("Girls", false);
            //GetComponent<AudioSource>().Stop();

            back = true;
        }
       

        else if (Input.GetKeyUp(KeyCode.S))
        {
          //  moveOn = 0.0f;
            if (anim.GetBool("MoveMode") == false)
                anim.SetFloat("Walk", moveOn);
            else if (anim.GetBool("MoveMode") == true)
                anim.SetFloat("Run", moveOn);

              a = Vector3.zero;
            overlap = false;
            //aa = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            playerSound.PlaySound();
            moveOn = 1.0f;
            if (anim.GetBool("MoveMode") == false)
                anim.SetFloat("Walk", moveOn);
            else if (anim.GetBool("MoveMode") == true)
                anim.SetFloat("Run", moveOn);

            anim.SetBool("Girls", false);
            //GetComponent<AudioSource>().Stop();

            back = true;
        }
        

        else if (Input.GetKeyUp(KeyCode.A))
        {
         //   moveOn = 0.0f;
            if (anim.GetBool("MoveMode") == false)
                anim.SetFloat("Walk", moveOn);
            else if (anim.GetBool("MoveMode") == true)
                anim.SetFloat("Run", moveOn);

               b = Vector3.zero;
            //bb = true;
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            playerSound.PlaySound();
            moveOn = 1.0f;
            if (anim.GetBool("MoveMode") == false)
                anim.SetFloat("Walk", moveOn);
            else if (anim.GetBool("MoveMode") == true)
                anim.SetFloat("Run", moveOn);

            anim.SetBool("Girls", false);
            //GetComponent<AudioSource>().Stop();

            back = true;
        }
        

        else if (Input.GetKeyUp(KeyCode.D))
        {
         //   moveOn = 0.0f;
            if (anim.GetBool("MoveMode") == false)
                anim.SetFloat("Walk", moveOn);
            else if (anim.GetBool("MoveMode") == true)
                anim.SetFloat("Run", moveOn);

            b = Vector3.zero;
            overlap2 = false;
           // bb = true;

        }

        if (Input.GetKey(KeyCode.W) )
        {
            if (overlap == true)
                a = -forward;
            else
                a = forward;
            //transform.rotation = Quaternion.LookRotation(forward);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            overlap = true;
            //transform.rotation = Quaternion.LookRotation(-forward);
            a = -forward;
        }
        else
        {
            aa = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (overlap2 == true)
                b = right;
            else
                b = -right;
            // transform.rotation = Quaternion.LookRotation(-right);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            overlap2 = true;
            //transform.rotation = Quaternion.LookRotation(right);
            b = right;
        }
        else
        {
            bb = true;
        }

        Quaternion haha;
        if (aa == true && bb == true)
        {
            //GetComponent<Rigidbody>().MoveRotation(initRot);
            //transform.rotation = initRot;
            haha = initRot;

            moveOn = 0.0f;
            if (anim.GetBool("MoveMode") == false)
                anim.SetFloat("Walk", moveOn);
            else if (anim.GetBool("MoveMode") == true)
                anim.SetFloat("Run", moveOn);
            playerSound.StopSound();
        }
        else
        {
            //GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(a + b));
            //transform.rotation = Quaternion.LookRotation(a + b);
            haha = Quaternion.LookRotation(a + b);
            //playerSound.PlaySound();
        }

        Quaternion newRotation = Quaternion.Lerp(transform.rotation, haha, 10.0f*Time.deltaTime);
        transform.rotation = newRotation;
//        GetComponent<Rigidbody>().MoveRotation(newRotation);


        //Vector3 move = new Vector3(0, 0, moveOn*moveSpeed);
        //로컬좌표를 월드좌표로 변경 (캐릭터가 바라보는 곳으로 이동하도록)
        //move = transform.TransformDirection(move);
        charController.Move((rightForce+forwardForce) * Time.deltaTime);
        //charController.Move(rightForce * Time.deltaTime);

    }

    void moveMode()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(anim.GetBool("MoveMode") == false)
            {
                anim.SetBool("MoveMode", true);
                
                if(anim.GetFloat("Walk") > 0.0f)
                {
                    anim.SetFloat("Walk", 0.0f);
                    anim.SetFloat("Run", 1.0f);
                }

                moveSpeed = 4.0f;
                rotSpeed = 4.0f;
            }
            else if(anim.GetBool("MoveMode") == true)
            {
                anim.SetBool("MoveMode", false);

                if (anim.GetFloat("Run") > 0.0f)
                {
                    anim.SetFloat("Walk", 1.0f);
                    anim.SetFloat("Run", 0.0f);
                }

                moveSpeed = 2.0f;
                rotSpeed = 2.0f;
            }

        }
    }

    public bool IsMoveBackward()
    {
        return back;
    }
    public void SetIsMoveBackward(bool a)
    {
        back = a;
    }

  //  public void OnControllerColliderHit(ControllerColliderHit aa) {}
}