using UnityEngine;
using System.Collections;

public class Combat : State
{
    private Monster monster;

    private float distance;

    private Vector3 objPos;
    private Vector3 tarPos;

    private AttackInfo attackInfo;

    public override void Enter(GameObject obj)
    {
        monster = obj.GetComponent<Monster>();
        ani = obj.GetComponent<Animator>();
        
        Debug.Log("현재 공격");
        obj.transform.GetChild(2).GetComponent<ChaseStopRange>().enabled = true;
        
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
        CollisionCheck(obj);

        objPos = obj.transform.position;
        tarPos = monster.Target.position;
        distance = Vector3.Distance(objPos, tarPos);

        if (distance < 3.0f)
        {
            //플레이어를 바라보도록 한다.
            Quaternion look = Quaternion.LookRotation((tarPos - objPos));
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, look, Time.deltaTime * 2.0f);

            if (attackInfo.progress == false)
            {
                attackInfo.count = 1;
                monster.AttackCheck.StartAttack();

                monster.NavAgent.ResetPath();
                ani.SetBool("walk", false);
                ani.SetBool("attack", true);
            }
            
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
        else
        {
            //추격(걷기) 상태로 변하면서 공격을 강제 종료 시켜야 되는 상황
            monster.AttackCheck.EndAttack();
        }

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
        {
            monster.NavAgent.SetDestination(tarPos);
        }
    }

    public override void Exit(GameObject obj)
    {
        Debug.Log("공격 종료");
        monster.AttackCheck.EndAttack();
        monster.NavAgent.ResetPath();
        obj.transform.GetChild(2).GetComponent<ChaseStopRange>().enabled = false;
        ani.SetBool("attack", false);

        monster.SetSoundDelay(monster.SoundDelay - 1);
    }

    public override void TriggerStay(GameObject obj, Collider col)
    {
        if (col.tag == "LanternLight")
        {
            //수정할 것
            //if (Cast(obj, col) == true)
            //{
            //    return;
            //}
            Debug.Log("도망 상태로 전환2");
            monster.GetStateMachine.ChangeState(new Flee());
        }
    }

    private void CollisionCheck(GameObject obj)
    {
        if (monster.Target.GetComponent<UnityStandardAssets.Characters
            .FirstPerson.FirstPersonController>().Invincible)
        {
            Debug.Log("플레이어 무적임으로 정찰 전환");
            monster.GetStateMachine.ChangeState(new Guard());
        }
        else if (obj.transform.GetChild(2).GetComponent<ChaseStopRange>().ChaseStop)
        {
            Debug.Log("정찰 상태로 전환");
            monster.GetStateMachine.ChangeState(new Guard());
        }
    }

    private bool Cast(GameObject obj, Collider col)
    {
        RaycastHit hit;
        
        Vector3 tarPos = col.transform.position;
        Vector3 objPos = obj.transform.position;

        Debug.DrawRay(objPos, tarPos - objPos, Color.gray);

        int layerMask = (1 << 8) | (1 << 10);
        layerMask = ~layerMask;

        bool result = Physics.Raycast(objPos, tarPos - objPos, out hit, (tarPos - objPos).magnitude, layerMask);
        return result;
    }
}

public class AttackInfo
{
    public bool progress;
    public int count;
    //public Animator ani;
}