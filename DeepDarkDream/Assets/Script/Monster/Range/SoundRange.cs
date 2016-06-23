using UnityEngine;
using System.Collections;

public class SoundRange : MonoBehaviour
{
    UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;

    private bool soundTarget = false;
    public bool SoundTarget { get { return soundTarget; } }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    void OnEnable()
    {
        soundTarget = false;
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "PlayerSound")
        {
            //if (player.WalkState == WALK_STATE.SLOW_WALK || player.WalkState == WALK_STATE.STOP)
            if(player.PlayerSoundRange == false)
            {
                return;
            }

            soundTarget = true;
        }
    }
}
