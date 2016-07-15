using UnityEngine;
using System.Collections;

public class Guard : State
{
    private Monster monster;
    private Animator ani;
    private int curWayPoint;
    private float distance;

    public override void Enter(GameObject obj)
    {
        monster = obj.GetComponent<Monster>();
        ani = monster.Ani;

        //Execute에서 하나를 더해주고 설정해주기 때문에
        curWayPoint = monster.WayPoints.Length;
        Debug.Log("현재 정찰");
        monster.VRange.enabled = true;
        monster.SRange.enabled = true;
        monster.CSRange.enabled = true;
        monster.Body.enabled = true;

        ani.SetBool("walk", true);
    }

    public override void Execute(GameObject obj)
    {
        //상태 변화가 있다면 바로 종료
        if (CollisionCheck(obj))
        {
            return;
        }

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return;
        }
        else if (monster.NavAgent.remainingDistance < 1.0f)
        {
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
        monster.VRange.enabled = false;
        monster.SRange.enabled = false;
        monster.CSRange.enabled = false;
        monster.Body.enabled = false;
    }

    private bool CollisionCheck(GameObject obj)
    {
        if (monster.Target.GetComponent<UnityStandardAssets.Characters
            .FirstPerson.FirstPersonController>().Invincible)
        {
            return true;
        }
        else if (monster.Body.BeShot)
        {
            Debug.Log("정찰에서 도망 상태로 전환");
            monster.GetStateMachine.ChangeState(new Flee());
            return true;
        }
        else if(monster.CSRange.InRange == false)
        {
            //추격범위를 넘어선 상태라면 발각 취소
            return false;
        }
        else if (monster.SRange.SoundTarget)
        {
            Debug.Log("소리 감지 공격 상태로 전환");
            monster.GetStateMachine.ChangeState(new Combat());
            return true;
        }
        else if (monster.VRange.VisualTarget)
        {
            Debug.Log("시각 감지 공격 상태로 전환");
            monster.GetStateMachine.ChangeState(new Combat());
            return true;
        }

        return false;
    }
}
