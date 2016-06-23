using UnityEngine;
using System.Collections;

public class Flee : State
{
    private Monster monster;
    //private int curWayPoint;
    private float distance;
    private int farIndex;

    public override void Enter(GameObject obj)
    {
        monster = obj.GetComponent<Monster>();
        ani = obj.GetComponent<Animator>();

        //curWayPoint = 0;
        monster.NavAgent.ResetPath();

        //플레이어와 가장 멀리 있는 waypoint를 찾아 이동한다
        //이미 가장 멀리 있는 waypoint에 있다면 다른 곳으로 이동하도록 한다.
        //이동하면서 플레이어와 접촉하면?
        float farDistance = 0.0f;
        farIndex = 0;
        //foreach (Transform point in monster.WayPoints)
        for(int i = 0; i < monster.WayPoints.Length; ++i)
        {
            distance = Vector3.Distance(monster.transform.position, monster.WayPoints[i].position);
            if(distance < 1.0f)
            {
                continue;
            }

            distance = Vector3.Distance(monster.Target.position, monster.WayPoints[i].position);
            if (farDistance < distance)
            {
                farDistance = distance;
                farIndex = i;

                ani.SetBool("walk", true);
                monster.NavAgent.SetDestination(monster.WayPoints[farIndex].position);
            }
        }
    }

    public override void Execute(GameObject obj)
    {
        distance = Vector3.Distance(obj.transform.position, monster.WayPoints[farIndex].position);

        if (distance < 1.0f)
        {
            Debug.Log("정찰 상태로 전환");
            monster.GetStateMachine.ChangeState(new Guard());
        }

        //ani.SetBool("walk", true);
        //monster.NavAgent.SetDestination(monster.WayPoints[farIndex].position);

        //Debug.DrawRay(obj.transform.position, monster.WayPoints[farIndex].position - obj.transform.position);
        //Debug.Log("현재 도망 중이야~");
    }

    public override void Exit(GameObject obj)
    {
        monster.NavAgent.ResetPath();
    }


    public override void TriggerEnter(GameObject obj, Collider col)
    {
    }
}
