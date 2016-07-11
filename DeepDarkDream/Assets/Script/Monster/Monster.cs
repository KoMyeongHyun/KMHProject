using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    //public Behaviors behavior = Behaviors.IDLE;

    [SerializeField]
    private Transform[] wayPoints;
    public Transform[] WayPoints { get { return wayPoints; } }

    [SerializeField]
    private MonsterAttackCheck attackCheck;
    public MonsterAttackCheck AttackCheck { get { return attackCheck; } }

    [SerializeField]
    private VisualRange vRange;
    public VisualRange VRange { get { return vRange; } }
    [SerializeField]
    private SoundRange sRange;
    public SoundRange SRange { get { return sRange; } }
    [SerializeField]
    private ChaseStopRange csRange;
    public ChaseStopRange CSRange { get { return csRange; } }
    [SerializeField]
    private MainBody body;
    public MainBody Body { get { return body; } }


    [SerializeField] private Animator ani;
    public Animator Ani { get { return ani; } }

    //[SerializeField]
    private NavMeshAgent navAgent;
    public NavMeshAgent NavAgent { get { return navAgent; } }
    private Transform target;
    public Transform Target { get { return target; } }

    private StateMachine stateMachine;
    public StateMachine GetStateMachine { get { return stateMachine; } }

    private SoundController sound;
    public SoundController Sound { get { return sound; } }
    private int soundDelay;
    public int SoundDelay { get { return soundDelay; }/* set { soundDelay = value; }*/ }
    public void SetSoundDelay(int a) { soundDelay = a; }

    // Use this for initialization
    void Start ()
    {
        vRange.enabled = false;
        sRange.enabled = false;
        csRange.enabled = false;
        
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player").transform;

        stateMachine = new StateMachine(gameObject, new Guard());

        sound = new SoundController(gameObject);
        soundDelay = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        stateMachine.RunBehaviors();
	}

    //상태별로 클래스를 나눠서 처리해야 됨
    public void OnTriggerEnter(Collider col)
    {
        stateMachine.TriggerEnter(gameObject, col);
    }

    public void OnTriggerStay(Collider col)
    {
        stateMachine.TriggerStay(gameObject, col);
    }

    public void OnTriggerExit(Collider col)
    {
        stateMachine.TriggerExit(gameObject, col);
    }
}
