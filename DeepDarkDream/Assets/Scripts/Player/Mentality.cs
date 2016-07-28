using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Collections;

public class Mentality
{
    public readonly float MAX_MENTALITY;
    private readonly float PENALTY_MENTALITY;

    private float currentMentality;
    public float CurrentMentality { get { return currentMentality; } }

    public Mentality()
    {
        JsonData root = SettingParser.Instance.SettingData;
        JsonData data = root["player"]["mentality"];
        MAX_MENTALITY = float.Parse(data["maxMentality"].ToString());
        float percentage = float.Parse(data["penaltyLine(%)"].ToString()) * 0.01f;
        PENALTY_MENTALITY = MAX_MENTALITY * percentage;

        currentMentality = MAX_MENTALITY;
    }

    public float RecoverMentality(float value)
    {
        float result = currentMentality + value;
        if (result > MAX_MENTALITY)
        {
            currentMentality = MAX_MENTALITY;
        }
        else
        {
            currentMentality = result;
        }
        PenaltyMentality();
        return currentMentality;
    }

    public float DamagedMentality(float value)
    {
        float result = currentMentality - value;
        if(result <= 0.0f)
        {
            //사망 처리
            currentMentality = 0.0f;
        }
        else
        {
            currentMentality = result;
        }

        
        PenaltyMentality();
        return currentMentality;
    }

    public void PenaltyMentality()
    {
        if (currentMentality < PENALTY_MENTALITY)
        {
            //패널티 적용
            GameObject.FindWithTag("PenaltyMentality").GetComponent<Animation>().Play();
        }
        else
        {
            //패널티 취소, 꺼주기
            GameObject.FindWithTag("PenaltyMentality").GetComponent<Animation>().Stop();
            GameObject.FindWithTag("PenaltyMentality").GetComponent<RawImage>().color 
                = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

}
