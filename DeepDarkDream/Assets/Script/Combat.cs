using UnityEngine;
using System.Collections;

public class Combat : State
{
    private Monster monster;

    private float distance;

    private Vector3 objPos;
    private Vector3 tarPos;

    private float attackTime;

    public override void Enter(GameObject obj)
    {
        monster = obj.GetComponent<Monster>();
        ani = obj.GetComponent<Animator>();

        monster.NavAgent.ResetPath();
        obj.transform.GetChild(2).GetComponent<ChaseStopRange>().enabled = true;

        attackTime = 0.0f;
    }

    public override void Execute(GameObject obj)
    {
        CollisionCheck(obj);

        objPos = obj.transform.position;
        tarPos = monster.Target.position;

        distance = Vector3.Distance(objPos, tarPos);

        if (distance < 3.0f)
        {
            //공격
            monster.NavAgent.ResetPath();

            //플레이어를 바라보도록 한다.
            Quaternion look = Quaternion.LookRotation((tarPos - objPos));
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, look, Time.deltaTime * 10.0f);

            ani.SetBool("walk", false);
            ani.SetBool("attack", true);
            attackTime += Time.deltaTime;
            
            if(attackTime > 1.7f)//공격 시간 임시 방편
            {
                attackTime = 0.0f;
                GameObject.FindGameObjectWithTag("Player").SendMessage("DamagedMentality", 70.0f);
            }
            return;
        }
        else if(distance > 20.0f)
        {
            Debug.Log("플레이어 너무 멀어서 정찰 상태로 전환");
            monster.GetStateMachine.ChangeState(new Guard());
        }
        else if (monster.Target.GetComponent<UnityStandardAssets.Characters
            .FirstPerson.FirstPersonController>().Invincible)
        {
            Debug.Log("플레이어 무적임으로 정찰 전환");
            monster.GetStateMachine.ChangeState(new Guard());
        }

        ani.SetBool("walk", true);
        ani.SetBool("attack", false);
        attackTime = 0.0f;
        monster.NavAgent.SetDestination(tarPos);
    }

    public override void Exit(GameObject obj)
    {
        monster.NavAgent.ResetPath();
        obj.transform.GetChild(2).GetComponent<ChaseStopRange>().enabled = false;
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
