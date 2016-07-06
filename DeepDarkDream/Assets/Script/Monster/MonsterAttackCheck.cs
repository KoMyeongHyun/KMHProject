using UnityEngine;
using System.Collections;

public class MonsterAttackCheck : MonoBehaviour
{
    [SerializeField]
    private Animator ani;


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PlayerOb")
        {
            //한번의 애니메이션당 한번만 동작하도록
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                //에러 발생 상황이 존재함, 캐스팅 실패 처리할 것
                Combat combat = (Combat)ani.transform.GetComponent<Monster>().GetStateMachine.GetCurState;
                if (combat.AttackCount > 0)
                {
                    Debug.Log("한대 타격 당함");
                    combat.ProcessAttackCount();
                    
                    GameObject.FindGameObjectWithTag("Player").SendMessage("BeShotFromMonster", 70.0f);
                }
            }
        }
    }
}
