using UnityEngine;
using System.Collections;

public class StaticDataLoad : MonoBehaviour
{
    public AudioClip lockDoor;
    public AudioClip usedKey;
    public AudioClip openDoor;
    public AudioClip obstacle1;
    public AudioClip breath;
    public AudioClip zombieSound;

    void Awake()
    {
        SoundPool.Instance.AddSoundClip("LockDoor", lockDoor);
        SoundPool.Instance.AddSoundClip("UsedKey", usedKey);
        SoundPool.Instance.AddSoundClip("OpenDoor", openDoor);
        SoundPool.Instance.AddSoundClip("Obstacle1", obstacle1);
        SoundPool.Instance.AddSoundClip("호흡소리", breath);
        SoundPool.Instance.AddSoundClip("ZombieSound", zombieSound);

        ItemFactory.Instance.RegisterType(ItemType.CONSUMPTION, new ConsumptionItemCreator());
        ItemFactory.Instance.RegisterType(ItemType.KIT, new ItemCreator());
        ItemFactory.Instance.RegisterType(ItemType.WEAPON, new ItemCreator());
        ItemFactory.Instance.RegisterType(ItemType.RECORD, new RecordItemCreator());

        //JSON GameSetting Load
        SettingParser.Instance.LoadSetting();
    }
}
