using UnityEngine;
using System.Collections;

public class Mentality
{
    public const float MAX_MENTALITY = 1000.0f;

    private float currentMentality;
    public float CurrentMentality { get { return currentMentality; } }

    public Mentality()
    {
        currentMentality = MAX_MENTALITY;
    }

    public float RecoverMentality(float value)
    {
        float result = currentMentality + value;
        if(result > MAX_MENTALITY)
        {
            currentMentality = MAX_MENTALITY;
            return currentMentality;
        }

        currentMentality = result;
        return currentMentality;
    }
    public float DamagedMentality(float value)
    {
        float result = currentMentality - value;
        if(result <= 0.0f)
        {
            //사망 처리
            currentMentality = 0.0f;
            return currentMentality;
        }

        currentMentality = result;
        return currentMentality;
    }

}
