using UnityEngine;
using System.Collections;

public class RuneStone : MonoBehaviour
{
    [SerializeField]
    private float delayTime;
    [SerializeField]
    private float invincibleTime;
    private bool available;

    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;

    void Start()
    {
        available = true;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag != "Player")
        {
            return;
        }

        if (Input.GetButtonDown("Fire1") && available)
        {
            //효과 발동
            //GameObject.FindGameObjectWithTag("Player").SendMessage("TouchRune", invincibleTime);
            StartCoroutine(InvincibleEffect(invincibleTime));
            StartCoroutine(DelayedStone());
        }
    }

    public IEnumerator InvincibleEffect(float _time)
    {
        float progressTime = 0.0f;
        while (progressTime < _time)
        {
            progressTime += Time.deltaTime;
            player.Invincible = true;
            yield return null;
        }

        player.Invincible = false;
        yield return null;
    }

    //일정 시간 사용 금지
    private IEnumerator DelayedStone()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
        gameObject.GetComponent<Light>().enabled = false;
        available = false;

        yield return new WaitForSeconds(delayTime);

        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.GetComponent<Light>().enabled = true;
        available = true;
    }
}