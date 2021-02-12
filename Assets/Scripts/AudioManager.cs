using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;

    [Range(0f, 2f)] public float volume = 1f;
    [Range(0f, 2f)] public float pitch  = 1f;

    [Range(0f, 0.25f)] public float randomVolumeAddOn = 0.25f;
    [Range(0f, 0.25f)] public float randomPitchAddOn  = 0.25f;

    public bool loop = false;

    AudioSource source;

    public void SetSource (AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play(Vector3 soundPosition)
    {
        source.volume = volume * (1 + Random.Range(-randomVolumeAddOn, randomVolumeAddOn)) * PlayerPrefsController.GetMasterVolume();
        source.pitch  = pitch * (1 + Random.Range(-randomPitchAddOn, randomPitchAddOn));
        
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [Header("Master Audio Mixer:")]
    public AudioMixerGroup masterAudioMixerGroup;

    [Header("All Sounds")]
    public List<Sound[]> allSounds = new List<Sound[]>();

    [Header("Ambient Sounds")]
    public Sound[] windSounds;

    [Header("Blunt Weapon Sounds")]
    public Sound[] bluntHitOpponentSounds;

    [Header("Bow and Arrow Sounds")]
    public Sound[] arrowHitOpponentSounds;
    public Sound[] arrowHitWallSounds;
    public Sound[] bowDrawSounds;
    public Sound[] bowReleaseSounds;

    [Header("Fireball Sounds")]
    public Sound[] fireballCastSounds;
    public Sound[] fireballHitSounds;

    [Header("Inventory Sounds")]
    public Sound[] goldSounds;

    [Header("Music Sounds")]
    public Sound[] musicSounds;
    public Sound[] victorySounds;

    [Header("Sword Sounds")]
    public Sound[] swordSlashOpponentSounds;
    public Sound[] swordStabOpponentSounds;

    [Header("Throw Sounds")]
    public Sound[] throwSounds;

    [Header("Voice Sounds")]
    public Sound[] humanMaleDeathSounds;
    public Sound[] humanMaleGruntSounds;

    void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        #endregion

        allSounds.Add(windSounds);
        allSounds.Add(bluntHitOpponentSounds);
        allSounds.Add(arrowHitOpponentSounds);
        allSounds.Add(arrowHitWallSounds);
        allSounds.Add(bowDrawSounds);
        allSounds.Add(bowReleaseSounds);
        allSounds.Add(fireballCastSounds);
        allSounds.Add(fireballHitSounds);
        allSounds.Add(goldSounds);
        allSounds.Add(musicSounds);
        allSounds.Add(victorySounds);
        allSounds.Add(swordSlashOpponentSounds);
        allSounds.Add(swordStabOpponentSounds);
        allSounds.Add(throwSounds);
        allSounds.Add(humanMaleDeathSounds);
        allSounds.Add(humanMaleGruntSounds);
    }

    void Start()
    {
        foreach (Sound[] soundArray in allSounds)
        {
            for (int i = 0; i < soundArray.Length; i++)
            {
                GameObject _go = new GameObject("Sound_" + i + " " + soundArray[i].soundName);
                _go.transform.SetParent(transform);
                soundArray[i].SetSource(_go.AddComponent<AudioSource>());
                _go.GetComponent<AudioSource>().outputAudioMixerGroup = masterAudioMixerGroup;
            }
        }

        PlayAmbienceSound();
    }

    public void PlaySound(Sound[] soundArray, string _soundName, Vector3 soundPosition)
    {
        for (int i = 0; i < soundArray.Length; i++)
        {
            if (soundArray[i].soundName == _soundName)
            {
                soundArray[i].Play(soundPosition);
                return;
            }
        }

        // No sound with _soundName
        Debug.LogWarning("AudioManager: Sound not found in list: " + _soundName);
    }

    public void PlayRandomSound(Sound[] soundArray)
    {
        int randomIndex = Random.Range(0, soundArray.Length);
        for (int i = 0; i < soundArray.Length; i++)
        {
            if (soundArray[randomIndex] == soundArray[i])
            {
                PlaySound(soundArray, soundArray[i].soundName, Vector3.zero);
                return;
            }
        }
    }

    public void StopSound(Sound[] soundArray, string _soundName)
    {
        for (int i = 0; i < soundArray.Length; i++)
        {
            if (soundArray[i].soundName == _soundName)
            {
                soundArray[i].Stop();
                return;
            }
        }

        // No sound with _soundName
        Debug.LogWarning("AudioManager: Sound not found in list: " + _soundName);
    }

    void PlayAmbienceSound()
    {
        int randomIndex = Random.Range(0, windSounds.Length);
        for (int i = 0; i < windSounds.Length; i++)
        {
            if (windSounds[randomIndex] == windSounds[i])
            {
                PlaySound(windSounds, windSounds[i].soundName, Vector3.zero);
                Invoke("PlayAmbienceSound", windSounds[i].clip.length);
                return;
            }
        }
    }

    public void PlayPickUpGoldSound(int goldAmount)
    {
        if (goldAmount <= 10)
            PlaySound(goldSounds, goldSounds[0].soundName, Vector3.zero);
        else if (goldAmount > 10 && goldAmount <= 50)
        {
            int randomNum = Random.Range(1, 3);
            if (randomNum == 1)
                PlaySound(goldSounds, goldSounds[1].soundName, Vector3.zero);
            else
                PlaySound(goldSounds, goldSounds[2].soundName, Vector3.zero);
        }
        else if (goldAmount > 50 && goldAmount <= 100)
            PlaySound(goldSounds, goldSounds[3].soundName, Vector3.zero);
        else
            PlaySound(goldSounds, goldSounds[4].soundName, Vector3.zero);
    }

    public void PlayMeleeHitSound(MeleeWeaponType weaponType)
    {
        if (weaponType == MeleeWeaponType.Blade)
            PlayRandomSound(swordSlashOpponentSounds);
        else if (weaponType == MeleeWeaponType.Blunt)
            PlayRandomSound(bluntHitOpponentSounds);
        else if (weaponType == MeleeWeaponType.Piercing)
            PlayRandomSound(swordStabOpponentSounds);
    }

    public void PlayRangedHitSound(RangedWeaponType weaponType, bool isAttackingCastle)
    {
        if (weaponType == RangedWeaponType.Fireball)
            PlayRandomSound(fireballHitSounds);
        else
        {
            if (isAttackingCastle == false)
                PlayRandomSound(arrowHitOpponentSounds);
            else
                PlayRandomSound(arrowHitWallSounds);
        }
    }

    public void PlayShootOrThrowSound(RangedWeaponType weaponType)
    {
        if (weaponType == RangedWeaponType.Bow || weaponType == RangedWeaponType.Crossbow)
            PlayRandomSound(bowReleaseSounds);
        else if (weaponType == RangedWeaponType.Spear)
            PlayRandomSound(throwSounds);
        else if (weaponType == RangedWeaponType.Fireball)
            PlayRandomSound(fireballCastSounds);
    }
}
