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

    [SerializeField]
    private GameObject beShot;
    private UnityStandardAssets.ImageEffects.MotionBlur motionBlur;
    private SoundController motionBlurSound;

    // Use this for initialization
    void Awake ()
    {
        stamina = new Stamina();
        mentality = new Mentality();
        lacklustreTime = 0.0f;

        staminaTrans = GameObject.FindGameObjectWithTag("Stamina").GetComponent<Transform>();
        mentalityTrans = GameObject.FindGameObjectWithTag("Mentality").GetComponent<Transform>();

        motionBlur = GameObject.FindGameObjectWithTag("MainCamera").
            GetComponent<UnityStandardAssets.ImageEffects.MotionBlur>();
        motionBlurSound = new SoundController(motionBlur.gameObject);

        NotificationCenter.DefaultCenter.AddObserver(this, "BeShotFromMonster");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //test
        if (Input.GetKeyDown(KeyCode.Q))
        { stamina.ChangeStaminaOfSpirit(mentality.DamagedMentality(300.0f)); }
        if (Input.GetKeyDown(KeyCode.E))
        { stamina.ChangeStaminaOfSpirit(mentality.RecoverMentality(300.0f)); }
        if (Input.GetKeyDown(KeyCode.R))
        { stamina.FullRecovery(); }

        bool result = stamina.CalculateStamina();
        if (result == false)
        {
            StartCoroutine(StandByRecovery());
        }


        //스테미너에서 한계상태 설정해주고 상태가 투르면 설정 켜주기 한번만 날려야 되는데
        //motionBlur.enabled = stamina.Penalty ? true : false;
        if(stamina.Penalty)
        {
            if(motionBlur.enabled == false)
            {
                motionBlur.enabled = true;
                motionBlurSound.ChangeAndPlay("호흡소리");
                motionBlurSound.SetRepeat(true);
            }
        }
        else
        {
            if(motionBlur.enabled)
            {
                motionBlur.enabled = false;
                motionBlurSound.StopSound();
                motionBlurSound.SetRepeat(false);
            }
        }

        //시간 계속 더하기 특정 시간 이상이면 피 깎는 코루틴 작동
        if (lacklustreTime < 0.0f)
        { }
        else
        {
            lacklustreTime += Time.deltaTime;
            if (lacklustreTime > 5.0f)
            {
                StartCoroutine(DecreaseMentality());
            }
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
        //빛에 닿아있으면 패널티 시간 초기화
        if(col.tag == "LanternLight")
        {
            lacklustreTime = 0.0f;
        }
    }

    public void RecoverMentalityForMedicine(float value)
    {
        stamina.ChangeStaminaOfSpirit(mentality.RecoverMentality(value));
    }

    public void DamagedMentality(float value)
    {
        stamina.ChangeStaminaOfSpirit(mentality.DamagedMentality(value));
    }

    public void BeShotFromMonster(NotificationCenter.Notification _notification) //함수 sender에 접근 가능
    {
        //피격 데미지 주기
        DamagedMentality((float)_notification.data["Damage"]);

        //화면에 피격 표시 해주기
        GameObject bs = Instantiate(beShot);
        bs.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>());
        bs.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        bs.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //화면 흔들어 주기
        gameObject.SendMessage("OnBeShotWave");
    }

    //void OnGUI()
    //{
        //GUI.TextField(new Rect(1000, 500, 100, 20), stamina.CurrentStamina + " / " + stamina.MaxStaminaOfSpirit);
    //}
}
