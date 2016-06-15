using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mentality
{
    public const float MAX_MENTALITY = 1000.0f;
    public const float PENALTY_MENTALITY = MAX_MENTALITY * 0.2f;

    private float currentMentality;
    public float CurrentMentality { get { return currentMentality; } }

    public Mentality()
    {
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
