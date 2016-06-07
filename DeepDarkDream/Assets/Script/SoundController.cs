using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundController
{
    private AudioSource audioSource;

    public SoundController(GameObject obj)
    {
        audioSource = obj.GetComponent<AudioSource>();
    }

    public void ChangeAndPlay(string clipName)
    {
        audioSource.clip = SoundPool.Instance.getSoundPool[clipName];
        audioSource.Play();
    }

    public void ChangeSound(string clipName)
    {
        audioSource.clip = SoundPool.Instance.getSoundPool[clipName];
    }
    public void PlaySound()
    {
        if (audioSource.isPlaying == false)
        {
            audioSource.Play();
        }
    }
    public void StopSound()
    {
        audioSource.Stop();
    }
}
