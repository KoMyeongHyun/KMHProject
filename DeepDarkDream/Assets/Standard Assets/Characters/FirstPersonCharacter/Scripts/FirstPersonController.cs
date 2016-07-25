using UnityEngine;
using System;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public enum WALK_STATE
{ STOP, SLOW_WALK, WALK, RUN, WALK_COUNT }

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_SlowWalkSpeed;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] private float m_KnockbackSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        private WALK_STATE walkState;
        public WALK_STATE WalkState { get { return walkState; } }
        private bool moveStop;
        public bool MoveStop { get { return moveStop; } }
        private bool zeroStamina;
        public bool ZeroStamina { get { return zeroStamina; } }
        public void setZeroStamina(bool zero) { zeroStamina = zero; }
        private SphereCollider soundRange;
        public bool PlayerSoundRange { get { return soundRange.enabled; } }
        private float lastClickedTime;
        private const float catchTime = 0.25f;
        private bool run;
        public bool Run { get { return run; } }
        private bool stopBehavior;
        public bool StopBehavior { get { return stopBehavior; } }
        public void setStopBehavior(bool stop) { stopBehavior = stop; }
        private bool stopMove;
        public bool StopMove { get; set; }
        public void setStopMove(bool stop) { stopMove = stop; }
        private bool knockback;
        public void setKnockback(bool stop) { knockback = stop; }
        private bool invincible;
        public bool Invincible { get; set; }


        //private SphereCollider soundRange;

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            //m_AudioSource = transform.GetChild(3).GetComponent<AudioSource>();
            m_MouseLook.Init(transform , m_Camera.transform);

            walkState = WALK_STATE.STOP;
            moveStop = true;
            zeroStamina = false;
            soundRange = GameObject.FindGameObjectWithTag("PlayerSound").GetComponent<SphereCollider>();
            lastClickedTime = 0.0f;
            run = false;
            stopBehavior = false;
            stopMove = false;
            knockback = false;
            invincible = false;

            DontDestroyOnLoad(this);
        }


        // Update is called once per frame
        private void Update()
        {
            //Debug.Log(invincible);

            if (stopBehavior)
                return;

            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

            DoubleInput();
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            //m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);

            if (stopBehavior || stopMove)
            {
                speed = 0.0f;
            }

            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);
            
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (m_Input.x == 0.0f && m_Input.y == 0.0f && m_Jumping == false)
            {
                soundRange.enabled = false;
                walkState = WALK_STATE.STOP;
                moveStop = true;
                run = false;
            }
            else
            {
                moveStop = false;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            if (walkState != WALK_STATE.SLOW_WALK)
            {
                PlayFootStepAudio();
            }
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            if(horizontal == 0.0f && vertical == 0.0f)
            {
                stopMove = false;
            }

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT

            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            
            m_IsWalking = !run;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                soundRange.enabled = false;
                walkState = WALK_STATE.SLOW_WALK;
                run = false;
            }
            else if (run)
            {
                soundRange.enabled = true;
                soundRange.radius = 15.0f;
                walkState = WALK_STATE.RUN;
            }
            else
            {
                if (moveStop)
                {
                    soundRange.enabled = false;
                    walkState = WALK_STATE.STOP;
                    run = false;
                }
                else
                {
                    soundRange.enabled = true;
                    soundRange.radius = 10.0f;
                    walkState = WALK_STATE.WALK;
                }
            }
#endif

            //Debug.Log(zeroStamina);
            if (zeroStamina)
            {
                walkState = WALK_STATE.WALK;
                m_IsWalking = true;
            }

            // set the desired speed to be walking or running
            if (walkState == WALK_STATE.SLOW_WALK)
                speed = m_SlowWalkSpeed;
            else
                speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;

            if (knockback)
                speed = m_KnockbackSpeed;

            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }

        private bool DoubleInput()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                float intervalTime = Time.time - lastClickedTime;
                //Debug.Log(intervalTime);
                if (intervalTime < catchTime)
                {
                    lastClickedTime = 0.0f;
                    run = true;
                    return true;
                }
                run = false;
                lastClickedTime = Time.time;
            }
            else if(Input.GetKeyUp(KeyCode.W))
            {
                run = false;
            }

            return false;
        }

        public void StopAndKnockback(Transform hit)
        {
            stopMove = true;
            StartCoroutine(Knockback(hit));
        }

        private IEnumerator Knockback(Transform hit)
        {
            //연산에 위치, 시간, 힘 필요
            float progressTime = 0.0f;
            Vector3 dir = transform.position - hit.position;
            dir.y = 0.0f;
            dir.Normalize();

            while (progressTime < 0.2f)
            {
                progressTime += Time.fixedDeltaTime;
                //m_CharacterController.Move(dir * 2.0f * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
        }

        //여기서 하면 조작 문제로 시간이 멈춘다.
        //public void TouchRune(float _time)
        //{
        //    StartCoroutine(InvincibleEffect(_time));
        //}

        //public IEnumerator InvincibleEffect(float _time)
        //{
        //    float progressTime = 0.0f;
        //    while(progressTime < _time)
        //    {
        //        Debug.Log(progressTime);
        //        Debug.Log(_time);
        //        progressTime += Time.deltaTime;
        //        invincible = true;
        //        yield return null;
        //    }
        //    Debug.Log("호출됨");
        //    invincible = false;
        //    yield return null;
        //}

        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }

        public void OnBeShotWave()
        {
            m_MouseLook.InitProcessDecayTime();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //문, 몬스터는 밀어버리고 진행하게끔 수정할 것 (문과 몬스터에 rigidbody 심어주고 컨트롤 할 것)
            //장애물에 닿으면 플레이어가 밀려야한다. 위, 아래 떨리지 않고

            //if(hit.gameObject.tag == "Obstacle")
            //{
            //    hit.gameObject.SendMessage("HitPlayer", transform);
            //}

            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
