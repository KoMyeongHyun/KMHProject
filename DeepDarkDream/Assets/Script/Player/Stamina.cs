using UnityEngine;
using System.Collections;

delegate bool InstructionStamina();

public class Stamina
{
    public const float MAX_STAMINA = 1000.0f;
    public const float PENALTY_STAMINA = MAX_STAMINA * 0.2f;

    private float maxStaminaOfSpirit;
    public float MaxStaminaOfSpirit { get { return maxStaminaOfSpirit; } }
    private float currentStamina;
    public float CurrentStamina { get { return currentStamina; } }
    private bool penalty;
    public bool Penalty { get { return penalty; } }

    private float consumeSpeed = 100.0f;
    private float consumeSpeedOfSlowWalk = 30.0f;
    private float recoverySpeedOfWalk = 10.0f;
    private float recoverySpeedOfStop = 30.0f;

    private InstructionStamina[] instructions;

    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;

    // Use this for initialization
    public Stamina()
    {
        maxStaminaOfSpirit = MAX_STAMINA;
        currentStamina = maxStaminaOfSpirit;
        penalty = false;

        instructions = new InstructionStamina[(int)WALK_STATE.WALK_COUNT];
        instructions[(int)WALK_STATE.STOP] = RecoverStaminaOfStop;
        instructions[(int)WALK_STATE.SLOW_WALK] = SpendStaminaOfSlowWalk;
        instructions[(int)WALK_STATE.WALK] = RecoverStaminaOfWalk;
        instructions[(int)WALK_STATE.RUN] = SpendStaminaOfRun;

        player = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    public bool CalculateStamina()
    {
        int curInst = (int)player.WalkState;
        bool result = instructions[curInst]();

        penalty = currentStamina < PENALTY_STAMINA ? true : false;

        return result;
        //스테미너 0되면 몇초 후에 가능하게 해주는건 코루틴 사용
    }

    public void ChangeStaminaOfSpirit(float spirit)
    {
        //정신력에 따른 맥스 스테미너 값 설정 (비율 방식으로 수정할 것)
        maxStaminaOfSpirit = spirit;

        instructions[(int)WALK_STATE.STOP] = InhibitRecovery;
        instructions[(int)WALK_STATE.WALK] = InhibitRecovery;
    }

    public void InhibitSpend()
    {
        //소비를 금지하면 달리기, 느리게 걷기 불가
        player.setZeroStamina(true);
    }

    public void PermitSpend()
    {
        player.setZeroStamina(false);
    }

    private bool SpendStaminaOfRun()
    {
        currentStamina -= consumeSpeed * Time.deltaTime;

        if (currentStamina < 0.0f)
        {
            currentStamina = 0.0f;
            return false;
        }

        return true;
    }
    private bool SpendStaminaOfSlowWalk()
    {
        currentStamina -= consumeSpeedOfSlowWalk * Time.deltaTime;

        if (currentStamina < 0.0f)
        {
            currentStamina = 0.0f;
            return false;
        }

        return true;
    }
    private bool RecoverStaminaOfWalk()
    {
        currentStamina += recoverySpeedOfWalk * Time.deltaTime;

        if (currentStamina > maxStaminaOfSpirit)
        {
            currentStamina = maxStaminaOfSpirit;
        }

        return true;
    }
    private bool RecoverStaminaOfStop()
    {
        currentStamina += recoverySpeedOfStop * Time.deltaTime;

        if (currentStamina > maxStaminaOfSpirit)
        {
            currentStamina = maxStaminaOfSpirit;
        }

        return true;
    }
    private bool InhibitRecovery()
    {
        //아래 조건을 만족할 때까지 회복 금지
        if(currentStamina <= maxStaminaOfSpirit)
        {
            instructions[(int)WALK_STATE.STOP] = RecoverStaminaOfStop;
            instructions[(int)WALK_STATE.WALK] = RecoverStaminaOfWalk;

            this.CalculateStamina();
        }

        return true;
    }
    private bool Dummy()
    {
        return true;
    }

    public void FullRecovery()
    {
        currentStamina = maxStaminaOfSpirit;
    }
}