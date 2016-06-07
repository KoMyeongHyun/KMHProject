using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour
{
    private Stamina stamina;
    private Mentality mentality;
    private float lacklustreTime;

    private Transform staminaTrans;
    public const float STAMINA_IMG_SIZE = 237.0f;

    //private Image mentalityImage;
    private Transform mentalityTrans;
    public const float MENTALITY_IMG_SIZE = 90.0f;

    // Use this for initialization
    void Awake ()
    {
        stamina = new Stamina();
        mentality = new Mentality();
        lacklustreTime = 0.0f;

        staminaTrans = GameObject.FindGameObjectWithTag("Stamina").GetComponent<Transform>();
        mentalityTrans = GameObject.FindGameObjectWithTag("Mentality").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //test
        if (Input.GetKeyDown(KeyCode.Q))
            stamina.ChangeStaminaOfSpirit(mentality.DamagedMentality(300.0f));
        if (Input.GetKeyDown(KeyCode.E))
            stamina.ChangeStaminaOfSpirit(mentality.RecoverMentality(100.0f));
        if (Input.GetKeyDown(KeyCode.R))
            stamina.FullRecovery();

        bool result = stamina.CalculateStamina();

        if (result == false)
        {
            StartCoroutine(StandByRecovery());
        }

        //시간 계속 더하기 특정 시간 이상이면 피 깎는 코루틴 작동
        if (lacklustreTime < 0.0f)
        { }
        else
        {
            lacklustreTime += Time.deltaTime;
            if (lacklustreTime > 5.0f)
                StartCoroutine(DecreaseMentality());
        }

        DisplayCondition();
	}

    private void DisplayCondition()
    {
        //STAMINA_IMG_SIZE일 때 stamina 0
        float staminaPos_x = STAMINA_IMG_SIZE - (stamina.CurrentStamina * STAMINA_IMG_SIZE / Stamina.MAX_STAMINA);
        staminaTrans.localPosition = new Vector3(staminaPos_x, staminaTrans.localPosition.y, staminaTrans.localPosition.z);

        float mentalityPos_y = MENTALITY_IMG_SIZE - (mentality.CurrentMentality * MENTALITY_IMG_SIZE / Mentality.MAX_MENTALITY);
        mentalityTrans.localPosition = new Vector3(mentalityTrans.localPosition.x, -mentalityPos_y, mentalityTrans.localPosition.z);
    }

    private IEnumerator StandByRecovery()
    {
        stamina.InhibitSpend();
        yield return new WaitForSeconds(5.0f);

        stamina.PermitSpend();
        yield return null;
    }

    private IEnumerator DecreaseMentality()
    {
        lacklustreTime = -1.0f;

        while(lacklustreTime < 0.0f)
        {
            stamina.ChangeStaminaOfSpirit(mentality.DamagedMentality(20.0f));
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    public void OnTriggerStay(Collider col)
    {
        if(col.tag == "LanternLight")
        {
            if (Cast(col) == true)
                return;

            lacklustreTime = 0.0f;
        }
    }

    private bool Cast(Collider col)
    {
        RaycastHit hit;

        Vector3 tarPos = col.transform.position;
        Vector3 objPos = this.transform.position;

        Debug.DrawRay(objPos, tarPos - objPos, Color.gray);

        int layerMask = (1 << 8) | (1 << 10);
        layerMask = ~layerMask;

        bool result = Physics.Raycast(objPos, tarPos - objPos, out hit, (tarPos - objPos).magnitude, layerMask);
        return result;
    }

    public void RecoverMentalityForMedicine(float value)
    {
        stamina.ChangeStaminaOfSpirit(mentality.RecoverMentality(value));
    }

    public void DamagedMentality(float value)
    {
        stamina.ChangeStaminaOfSpirit(mentality.DamagedMentality(value));
    }

    void OnGUI()
    {
        //GUI.TextField(new Rect(1000, 500, 100, 20), stamina.CurrentStamina + " / " + stamina.MaxStaminaOfSpirit);
    }
}
