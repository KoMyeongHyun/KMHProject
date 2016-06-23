using UnityEngine;
using System.Collections;

public class Combat : State
{
    private Monster monster;

    private float distance;

    private Vector3 objPos;
    private Vector3 tarPos;

    private int attackCount;
    public int AttackCount { get { return attackCount; } }
    public void ProcessAttackCount()
    {
        attackCount--;
    }

    public override void Enter(GameObject obj)
    {
        monster = obj.GetComponent<Monster>();
        ani = obj.GetComponent<Animator>();

        monster.NavAgent.ResetPath();
        obj.transform.GetChild(2).GetComponent<ChaseStopRange>().enabled = true;
        
        attackCount = 1;

        if (monster.SoundDelay <= 0)
        {
            monster.Sound.ChangeAndPlay("ZombieSound");
            monster.SetSoundDelay(3);
        }
    }

    public override void Execute(GameObject obj)
    {
        CollisionCheck(obj);

        if (monster.Target.GetComponent<UnityStandardAssets.Characters
            .FirstPerson.FirstPersonController>().Invincible)
        {
            //충돌 작업 끝내고 이부분 맨 위로 올릴 것
            Debug.Log("플레이어 무적임으로 정찰 전환");
            monster.GetStateMachine.ChangeState(new Guard());
        }

        objPos = obj.transform.position;
        tarPos = monster.Target.position;
        distance = Vector3.Distance(objPos, tarPos);

        if (distance < 3.0f)
        {
            //플레이어를 바라보도록 한다.
            Quaternion look = Quaternion.LookRotation((tarPos - objPos));
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, look, Time.fixedDeltaTime * 10.0f);

            //경로 초기화
            monster.NavAgent.ResetPath();

            //한 번만 공격할 것
            //attack이 아니면 attack 만들어 주고 공격진행변수true
            
            if(ani.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
            {
                ani.SetBool("attack", true);
                ani.SetBool("walk", false);
                attackCount = 1;
            }

            //if (attackTime == 0.0f)
            //{
            //    ani.SetBool("walk", false);
            //    ani.SetBool("attack", true);
            //}
            //attackTime += Time.fixedDeltaTime;

            //if(attackTime > 1.7f)//공격 시간 임시 방편
            //{
            //    attackTime = 0.0f;
            //    GameObject.FindGameObjectWithTag("Player").SendMessage("DamagedMentality", 70.0f);
            //}
            return;
        }
        else if(distance > 20.0f)
        {
            Debug.Log("플레이어 너무 멀어서 정찰 상태로 전환");
            monster.GetStateMachine.ChangeState(new Guard());
        }
        
        if(ani.GetCurrentAnimatorStateInfo(0).IsName("Walk") == false)
        {
            ani.SetBool("attack", false);
            ani.SetBool("walk", true);
        }
        monster.NavAgent.SetDestination(tarPos);
    }

    public override void Exit(GameObject obj)
    {
        monster.NavAgent.ResetPath();
        obj.transform.GetChild(2).GetComponent<ChaseStopRange>().enabled = false;

        ani.SetBool("attack", false);

        monster.SetSoundDelay(monster.SoundDelay - 1);
    }

    private void CollisionCheck(GameObject obj)
    {
        if (obj.transform.GetChild(2).GetComponent<ChaseStopRange>().ChaseStop)
        {
            Debug.Log("정찰 상태로 전환");
            monster.GetStateMachine.ChangeState(new Guard());
        }
    }

    public override void TriggerStay(GameObject obj, Collider col)
    {
        //만약 공격 애니메이션일 때, PlayerOb와 충돌하면 플레이어 체력 떨구기

        if (col.tag == "LanternLight")
        {
            if (Cast(obj, col) == true)
                return;

            Debug.Log("도망 상태로 전환2");
            monster.GetStateMachine.ChangeState(new Flee());
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
