﻿using UnityEngine;
using System.Collections;

public class Guard : State
{
    private Monster monster;
    private int curWayPoint;
    private float distance;

    public override void Enter(GameObject obj)
    {
        monster = obj.GetComponent<Monster>();
        ani = obj.GetComponent<Animator>();

        //Execute에서 하나를 더해주고 설정해주기 때문에
        curWayPoint = monster.WayPoints.Length;
        Debug.Log("현재 정찰");
        obj.transform.GetChild(0).GetComponent<VisualRange>().enabled = true;
        obj.transform.GetChild(1).GetComponent<SoundRange>().enabled = true;

        ani.SetBool("walk", true);
    }

    public override void Execute(GameObject obj)
    {
        CollisionCheck(obj);

        if (monster.NavAgent.remainingDistance < 1.0f)
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }

            //리버스 기능 추가 할 것
            ++curWayPoint;
            if (curWayPoint >= monster.WayPoints.Length)
            {
                curWayPoint = 0;
            }
            monster.NavAgent.SetDestination(monster.WayPoints[curWayPoint].position);
        }
    }

    public override void Exit(GameObject obj)
    {
        Debug.Log("정찰 종료");
        monster.NavAgent.ResetPath();
        obj.transform.GetChild(0).GetComponent<VisualRange>().enabled = false;
        obj.transform.GetChild(1).GetComponent<SoundRange>().enabled = false;
    }

    public override void TriggerStay(GameObject obj, Collider col)
    {
        if (col.tag == "LanternLight")
        {
            if (Cast(obj, col) == true)
            {
                return;
            }
            Debug.Log("정찰에서 도망 상태로 전환");
            monster.GetStateMachine.ChangeState(new Flee());
        }
    }

    private void CollisionCheck(GameObject obj)
    {
        if (monster.Target.GetComponent<UnityStandardAssets.Characters
            .FirstPerson.FirstPersonController>().Invincible)
        {
            return;
        }
        else if (obj.transform.GetChild(1).GetComponent<SoundRange>().SoundTarget)
        {
            Debug.Log("소리 감지 공격 상태로 전환");
            monster.GetStateMachine.ChangeState(new Combat());
        }
        else if (obj.transform.GetChild(0).GetComponent<VisualRange>().VisualTarget)
        {
            Debug.Log("시각 감지 공격 상태로 전환");
            monster.GetStateMachine.ChangeState(new Combat());
        }
    }

    private bool Cast(GameObject obj, Collider col)
    {
        RaycastHit hit;

        Vector3 tarPos = col.transform.position;
        Vector3 objPos = obj.transform.position;

        //Debug.DrawRay(objPos, tarPos - objPos, Color.gray);

        int layerMask = (1 << 8) | (1 << 10);
        layerMask = ~layerMask;

        bool result = Physics.Raycast(objPos, tarPos - objPos, out hit, (tarPos - objPos).magnitude, layerMask);
        return result;
    }
}
