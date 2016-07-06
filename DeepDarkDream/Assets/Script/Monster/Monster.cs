﻿using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    //public Behaviors behavior = Behaviors.IDLE;

    [SerializeField]
    private Transform[] wayPoints;
    public Transform[] WayPoints { get { return wayPoints; } }

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
        //behavior = Behaviors.GUARD;
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
