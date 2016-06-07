using UnityEngine;
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
        
        curWayPoint = 0;
        monster.NavAgent.ResetPath();
        obj.transform.GetChild(0).GetComponent<VisualRange>().enabled = true;
        obj.transform.GetChild(1).GetComponent<SoundRange>().enabled = true;
    }

    public override void Execute(GameObject obj)
    {
        CollisionCheck(obj);

        distance = Vector3.Distance(obj.transform.position, monster.WayPoints[curWayPoint].position);

        if (distance < 1.0f)
        {
            //리버스 기능 추가 할 것
            ++curWayPoint;
            if (curWayPoint >= monster.WayPoints.Length)
            {
                curWayPoint = 0;
            }
        }

        ani.SetBool("walk", true);
        monster.NavAgent.SetDestination(monster.WayPoints[curWayPoint].position);
    }

    public override void Exit(GameObject obj)
    {
        monster.NavAgent.ResetPath();
        obj.transform.GetChild(0).GetComponent<VisualRange>().enabled = false;
        obj.transform.GetChild(1).GetComponent<SoundRange>().enabled = false;
    }

    private void CollisionCheck(GameObject obj)
    {
        if (monster.Target.GetComponent<UnityStandardAssets.Characters
            .FirstPerson.FirstPersonController>().Invincible)
        {
            return;
        }

        if (obj.transform.GetChild(1).GetComponent<SoundRange>().SoundTarget)
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

    public override void TriggerStay(GameObject obj, Collider col)
    {
        if (col.tag == "LanternLight")
        {
            if (Cast(obj, col) == true)
                return;

            Debug.Log("도망 상태로 전환1");
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
