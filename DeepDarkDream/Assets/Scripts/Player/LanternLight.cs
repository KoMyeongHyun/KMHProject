using UnityEngine;
using LitJson;
using System.Collections;

public class LanternLight : MonoBehaviour
{
    private float MAX_ENERGY;
    private float PENALRY_ENERGY;
    private float CONSUME_SPEED;
    private float CONSUME_POWER_ON;
    //{
    //    get
    //    {
    //        JsonData root = SettingParser.Instance.SettingData;
    //        JsonData data = root["player"]["lantern"];
    //        return float.Parse(data["consumePowerOn"].ToString());
    //    }
    //}
    private float PRIME_INTENSITY;
    private float SUB_INTENSITY;

    private Light primeLight;
    private Light subLight;

    private float energy;
    public bool lightPower;

    private Animator ani;
    //private Transform oilTrans;

    //LanternLight()
    //{
        //JsonData root = SettingParser.Instance.SettingData;
        //JsonData data = root["player"]["lantern"];
    //}
    
    void Start ()
    {
        primeLight = gameObject.GetComponent<Light>();
        subLight = transform.GetChild(0).GetComponent<Light>();

        JsonData root = SettingParser.Instance.SettingData;
        JsonData data = root["player"]["lantern"];
        MAX_ENERGY = float.Parse(data["maxEnergy"].ToString());
        float percentage = float.Parse(data["penaltyLine(%)"].ToString()) * 0.01f;
        PENALRY_ENERGY = MAX_ENERGY * percentage;
        CONSUME_SPEED = float.Parse(data["consumeSpeed"].ToString());
        CONSUME_POWER_ON = float.Parse(data["consumePowerOn"].ToString());
        PRIME_INTENSITY = float.Parse(data["primeLightIntensity"].ToString());
        SUB_INTENSITY = float.Parse(data["subLightIntensity"].ToString());

        primeLight.intensity = PRIME_INTENSITY;
        subLight.intensity = SUB_INTENSITY;

        energy = MAX_ENERGY;
        lightPower = true;

        ani = gameObject.GetComponent<Animator>();
        //oilTrans = GameObject.FindGameObjectWithTag("Oil").GetComponent<Transform>();
        //ani.SetBool("penalty", false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //에너지가 적으면 빛이 약해지거나 불규칙하게 깜빡이거나 추후
        //킬 때 고정적으로 게이지 소모
	    if(energy > 0.0f && lightPower == true)
        {
            if(energy < PENALRY_ENERGY)
            {
                ani.enabled = true;
                ani.SetBool("penalty", true);
            }
            else
            {
                ani.enabled = false;
                primeLight.intensity = PRIME_INTENSITY;
                primeLight.enabled = true;
                subLight.intensity = SUB_INTENSITY;
                subLight.enabled = true;
            }

            energy -= CONSUME_SPEED * Time.deltaTime;
            if (energy < 0.0f)
            {
                energy = 0.0f;

                ani.enabled = false;
                primeLight.intensity = 0f;
                primeLight.enabled = true;
                subLight.intensity = 0f;
                subLight.enabled = true;
            }
        }
        else
        {
            primeLight.intensity = 0.0f;
            subLight.intensity = 0.0f;
            lightPower = false;
            this.GetComponent<SphereCollider>().enabled = false;
            //ani.SetBool("penalty", false);
        }

        if( Input.GetKeyDown(KeyCode.F) )
        {
            //어처피 0이면 출력도 안함
            if (energy <= 0.0f)
                return;

            if(lightPower == false)
            {
                if(energy < CONSUME_POWER_ON)
                    return;
                else
                    energy -= CONSUME_POWER_ON;
            }
            
            lightPower = !( lightPower );

            primeLight.intensity = (energy > 0) ? PRIME_INTENSITY : 0.0f;
            subLight.intensity = (energy > 0) ? SUB_INTENSITY : 0.0f;
            this.GetComponent<SphereCollider>().enabled = lightPower;

            if(lightPower == false)
            {
                ani.enabled = false;
                primeLight.intensity = PRIME_INTENSITY;
                primeLight.enabled = true;
                subLight.intensity = SUB_INTENSITY;
                subLight.enabled = true;
            }
        }
        else if(Input.GetKeyDown(KeyCode.T))
        {
            energy -= 50.0f;

            if (energy < 0.0f)
            {
                energy = 0.0f;

                ani.enabled = false;
                primeLight.intensity = 0f;
                primeLight.enabled = true;
                subLight.intensity = 0f;
                subLight.enabled = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            energy = MAX_ENERGY;
        }

        //DisplayOil();
    }

    //private void DisplayOil()
    //{
    //    float oilPos_x = OIL_IMG_SIZE - (energy * OIL_IMG_SIZE / MAX_ENERGY);
    //    oilTrans.localPosition = new Vector3(oilPos_x, oilTrans.localPosition.y, oilTrans.localPosition.z);
    //}
}
