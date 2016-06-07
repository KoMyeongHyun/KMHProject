using UnityEngine;
using System.Collections;

public class LanternLight : MonoBehaviour {

    public const float MAX_ENERGY = 1000.0f;
    public const float OIL_IMG_SIZE = 234.0f;

    private Light primeLight;
    private Light subLight;

    private float energy;
    private float consumeSpeed;
    private float consumePowerOn;

    public bool lightPower;

    private Transform oilTrans;

    // Use this for initialization
    void Start ()
    {
        primeLight = gameObject.GetComponent<Light>();
        subLight = transform.GetChild(0).GetComponent<Light>();
        primeLight.intensity = 7.0f; //조절 가능하게 뺄 것
        subLight.intensity = 7.15f;
        energy = MAX_ENERGY;       //조절 가능하게 뺄 것
        consumeSpeed = 2.0f;    //조절 가능하게 뺄 것
        consumePowerOn = 100.0f;
        lightPower = true;
        oilTrans = GameObject.FindGameObjectWithTag("Oil").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //에너지가 적으면 빛이 약해지거나 불규칙하게 깜빡이거나 추후
        //킬 때 고정적으로 게이지 소모
	    if(energy > 0.0f && lightPower == true)
        {
            energy -= consumeSpeed*Time.deltaTime;
            if (energy < 0.0f)
                energy = 0.0f;
        }
        else
        {
            primeLight.intensity = 0.0f;
            subLight.intensity = 0.0f;
            lightPower = false;
        }

        if( Input.GetKeyDown(KeyCode.F) )
        {
            if(lightPower == false)
            {
                if(energy < consumePowerOn)
                    return;
                else
                    energy -= consumePowerOn;
            }
            
            lightPower = !( lightPower );

            primeLight.intensity = (energy > 0) ? 7.0f : 0.0f;
            subLight.intensity = (energy > 0) ? 7.15f : 0.0f;
            this.GetComponent<SphereCollider>().enabled = lightPower;
        }
        //Debug.Log(energy);

        DisplayOil();
    }

    private void DisplayOil()
    {
        float oilPos_x = OIL_IMG_SIZE - (energy * OIL_IMG_SIZE / MAX_ENERGY);
        oilTrans.localPosition = new Vector3(oilPos_x, oilTrans.localPosition.y, oilTrans.localPosition.z);
    }
}
