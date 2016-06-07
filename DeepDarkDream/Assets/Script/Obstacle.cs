using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float knockbackTime;
    [SerializeField]
    private float knockbackPower;
    [SerializeField]
    protected string[] animationName;

    private Transform center;

    // Use this for initialization
    protected virtual void Start()
    {
        gameObject.GetComponent<Animation>().wrapMode = WrapMode.Loop;
        center = transform.GetChild(1);
    }

    void HitPlayer(Transform hit)
    {
        //플레이어가 움직이지 않으면 충돌이 안되는거 해결해야 됨
        GameObject.FindGameObjectWithTag("Player").SendMessage("DamagedMentality", damage);
        StartCoroutine(KnockbackPlayer(hit));
        StartCoroutine(StunPlayer(hit));
    }

    IEnumerator KnockbackPlayer(Transform hit)
    {
        float progressTime = 0.0f;

        while(progressTime < knockbackTime)
        {
            Vector3 dir = hit.position - center.position;
            dir.y = 0.0f;
            dir.Normalize();
            hit.GetComponent<CharacterController>().Move(dir * knockbackPower * Time.fixedDeltaTime);

            progressTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator StunPlayer(Transform hit)
    {
        float progressTime = 0.0f;
        while(progressTime < 1.0f)
        {
            progressTime += Time.deltaTime;
            hit.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().setKnockback(true);
            yield return null;
        }

        hit.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().setKnockback(false);        
    }

    void OnCollisionStay(Collision col)
    {
        if (col.transform.tag == "Interact")
        {
            gameObject.GetComponent<Animation>()[animationName[0]].speed = 0.0f;
        }
    }
    void OnCollisionExit(Collision col)
    {
        gameObject.GetComponent<Animation>()[animationName[0]].speed = 1.0f;
    }
}
