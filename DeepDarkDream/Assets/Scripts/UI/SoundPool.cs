using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundPool// : MonoBehaviour
{
    private static SoundPool instance = null;
    public static SoundPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SoundPool();
            }
            return instance;
        }
    }
    //monobehavior를 상속받는 클래스를 singleton 만들려면 오브젝트를 생성한 후 스크립트를 추가하고(AddComponent)
    //그 오브젝트의 스크립트를 가져오는 방식으로 만들면 된다.
    //private static SoundPool instance = null;
    //public static SoundPool Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            GameObject obj = new GameObject("Sound Pool");
    //            instance = obj.AddComponent<SoundPool>();
    //        }
    //        return instance;
    //    }
    //}


    private Dictionary<string, AudioClip> soundPool = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> getSoundPool { get { return soundPool; } }

    public void AddSoundClip(string clipName, AudioClip clipSound)
    {
        soundPool.Add(clipName, clipSound);
    }
}
