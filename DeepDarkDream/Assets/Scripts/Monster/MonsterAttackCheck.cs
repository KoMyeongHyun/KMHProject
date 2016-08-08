using UnityEngine;
using System.Collections;

public class MonsterAttackCheck : MonoBehaviour
{
    private Animator ani;
    private AttackInfo attackInfo;

    public void SetAttackCheckInfo(Animator _ani, AttackInfo _attackInfo)
    {
        ani = _ani;
        attackInfo = _attackInfo;
    }

    void Start()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void StartAttack()
    {
        attackInfo.progress = true;
        ani.SetBool("walk", false);
        ani.SetBool("attack", true);

        Debug.Log("공격 1번동작");
        StartCoroutine(ProceedAttack());
    }

    IEnumerator ProceedAttack()
    {
        while(ani.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
        {
            //공격을 강제 종료 시켰을 때 빠져나온다.
            if(attackInfo.progress == false)
            {
                yield break;
            }
            yield return null;
        }
        
        gameObject.GetComponent<BoxCollider>().enabled = true;

        Debug.Log("공격 2번동작");
        while (ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (attackInfo.progress == false)
            {
                yield break;
            }
            yield return null;
        }

        EndAttack();
    }

    public void EndAttack()
    {
        attackInfo.progress = false;
        ani.SetBool("attack", false);
        //ani.SetBool("walk", true);
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "PlayerOb")
        {
            //한번의 액션 애니메이션에 count 만큼 타격이 가능하도록
            //공격 애니메이션 마무리 부분에서 공격이 이뤄지지 않도록
            if(attackInfo.count > 0 && ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.65f)
            {
                --attackInfo.count;

                Hashtable data = new Hashtable();
                data.Add("Damage", 70.0f);
                NotificationCenter.DefaultCenter.PostNotification(this, "BeShotFromMonster", data);
                //GameObject.FindGameObjectWithTag("Player").SendMessage("BeShotFromMonster", 70.0f);
                Debug.Log("플레이어 타격 당함");
            }
        }
    }

}
