using UnityEngine;
using System.Collections;

public class Flee : State
{
    private Monster monster;
    private float distance;
    private int farIndex;

    public override void Enter(GameObject obj)
    {
        monster = obj.GetComponent<Monster>();
        ani = obj.GetComponent<Animator>();
        farIndex = 0;

        //몬스터 자기 자신과 가장 멀리 있는 waypoint를 찾아 이동한다
        //기존에는 플레이어와 가장 멀리 떨어진 waypoint를 찾아 이동했었음
        //현 문제점 : 도망인데 플레이어에게 다가오는 느낌을 줄 수 있다.
        float farDistance = -1.0f;
        for (int i = 0; i < monster.WayPoints.Length; ++i)
        {
            distance = Vector3.Distance(monster.transform.position, monster.WayPoints[i].position);
            if(farDistance < distance)
            {
                farDistance = distance;
                farIndex = i;
            }
        }

        //Execute에서 하나를 더해주고 설정해주기 때문에
        --farIndex;
        if(farIndex < 0)
        {
            farIndex = monster.WayPoints.Length - 1;
        }

        ani.SetBool("walk", true);
        Debug.Log("도망 시작 " + farIndex);
    }

    public override void Execute(GameObject obj)
    {
        if (monster.NavAgent.remainingDistance < 1.0f)
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }

            //Destination과 WayPoints[farIndex]의 거리가 1.0f보다 작지 않다면 다른 경로로 재설정
            distance = Vector3.Distance(monster.NavAgent.destination, monster.WayPoints[farIndex].position);
            if(distance > 1.0f)
            {
                ++farIndex;
                if(farIndex >= monster.WayPoints.Length)
                {
                    farIndex = 0;
                }
                monster.NavAgent.SetDestination(monster.WayPoints[farIndex].position);
            }
            else
            {
                Debug.Log("도망에서 정찰 상태로 전환");
                monster.GetStateMachine.ChangeState(new Guard());
            }
        }
    }

    public override void Exit(GameObject obj)
    {
        Debug.Log("도망 종료");
        monster.NavAgent.ResetPath();
    }
}
