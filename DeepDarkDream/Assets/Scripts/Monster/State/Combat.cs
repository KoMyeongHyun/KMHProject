﻿using UnityEngine;
using System.Collections;

public class Combat : State
{
    private Monster monster;
    private Animator ani;

    private float distance;

    private Vector3 objPos;
    private Vector3 tarPos;

    private AttackInfo attackInfo;

    public override void Enter(GameObject obj)
    {
        monster = obj.GetComponent<Monster>();
        ani = monster.Ani;
        
        Debug.Log("현재 공격");
        monster.CSRange.enabled = true;
        monster.Body.enabled = true;

        attackInfo = new AttackInfo();
        attackInfo.progress = false;
        attackInfo.count = 1;

        monster.AttackCheck.SetAttackCheckInfo(ani, attackInfo);

        if (monster.SoundDelay <= 0)
        {
            monster.Sound.ChangeAndPlay("ZombieSound");
            monster.SetSoundDelay(3);
        }
    }

    public override void Execute(GameObject obj)
    {
        //상태 변화가 있다면 바로 종료
        if( CollisionCheck(obj) )
        {
            return;
        }

        objPos = obj.transform.position;
        tarPos = monster.Target.position;
        distance = Vector3.Distance(objPos, tarPos);

        if (distance < 3.0f)
        {
            //공격 진행 중이라면 리턴
            if (attackInfo.progress)
            {
                return;
            }

            tarPos.y = objPos.y;
            Vector3 dest = (tarPos - objPos).normalized;
            float permissionVal = Vector3.Dot(obj.transform.forward, dest);
            if (permissionVal > 0.98f)
            {
                    attackInfo.count = 1;
                    monster.NavAgent.ResetPath();
                    monster.AttackCheck.StartAttack();
            }
            else
            {
                if (ani.GetCurrentAnimatorStateInfo(0).IsName("Walk") == false)
                {
                    ani.SetBool("walk", true);
                }
                monster.NavAgent.SetDestination(tarPos);
            }
        }
        else if(distance > 20.0f)
        {
            Debug.Log("플레이어 너무 멀어서 정찰 상태로 전환");
            monster.GetStateMachine.ChangeState(new Guard());
        }
        else
        {
            //공격 중지 후 걷기 상태가 되면 플레이어에게 접근하기
            if (attackInfo.progress == false)
            {
                if(ani.GetCurrentAnimatorStateInfo(0).IsName("Walk") == false)
                {
                    ani.SetBool("walk", true);
                }
                else
                {
                    monster.NavAgent.SetDestination(tarPos);
                    monster.NavAgent.updatePosition = true;
                }
            }
        }
    }

    public override void Exit(GameObject obj)
    {
        Debug.Log("공격 종료");
        monster.NavAgent.ResetPath();
        monster.NavAgent.updatePosition = true;
        monster.CSRange.enabled = false;
        monster.Body.enabled = false;

        monster.AttackCheck.EndAttack();
        ani.SetBool("attack", false);
        ani.SetBool("walk", false);

        monster.SetSoundDelay(monster.SoundDelay - 1);
    }

    private bool CollisionCheck(GameObject obj)
    {
        if (monster.Target.GetComponent<UnityStandardAssets.Characters
            .FirstPerson.FirstPersonController>().Invincible)
        {
            Debug.Log("플레이어 무적임으로 정찰 전환");
            monster.GetStateMachine.ChangeState(new Guard());
            return true;
        }
        else if (monster.Body.BeShot)
        {
            Debug.Log("공격 중 빛에 닿음 도망 상태로 전환");
            monster.GetStateMachine.ChangeState(new Flee());
            return true;
        }
        else if (monster.CSRange.ChaseStop)
        {
            Debug.Log("플레이어 놓침 정찰 상태로 전환");
            monster.GetStateMachine.ChangeState(new Guard());
            return true;
        }

        //장애물이 가로막고 있는 상황 추가해야 됨

        return false;
    }
}

public class AttackInfo
{
    public bool progress;
    public int count;
}