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
        source.volume = volume * (1 + Random.Range(-randomVolumeAddOn, randomVolumeAddOn));
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

    [Header("Audio Mixer:")]
    public AudioMixer masterMixer;
    public AudioMixerGroup masterAudioMixerGroup;
    public AudioMixerGroup musicAudioMixerGroup;
    public AudioMixerGroup effectsAudioMixerGroup;
    public AudioMixerGroup voicesAudioMixerGroup;

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

    [Header("Magic Sounds")]
    public Sound[] fireballCastSounds;
    public Sound[] fireballHitSounds;
    public Sound[] healingOrbCastSounds;
    public Sound[] healingOrbHitSounds;
    public Sound[] resurrectSounds;

    [Header("Music Sounds")]
    public Sound[] splashScreenMusicSounds;
    public Sound[] musicSounds;
    public Sound[] victorySounds;
    public Sound[] failSounds;

    [Header("Pickaxe Sounds")]
    public Sound[] pickaxeSounds;
    public Sound[] rockSmashSounds;

    [Header("Structure Sounds")]
    public Sound[] woodBreakingSounds;

    [Header("Sword Sounds")]
    public Sound[] swordSlashOpponentSounds;
    public Sound[] swordStabOpponentSounds;

    [Header("Throw Sounds")]
    public Sound[] throwSounds;

    [Header("UI Sounds")]
    public Sound[] buttonClickSounds;
    public Sound[] goldSounds;

    [Header("Voice Sounds")]
    public Sound[] humanMaleDeathSounds;
    public Sound[] humanFemaleDeathSounds;
    public Sound[] lichDeathSounds;
    public Sound[] skeletonDeathSounds;
    public Sound[] zombieDeathSounds;

    bool playedFirstSong;

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
            DontDestroyOnLoad(this);
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
        allSounds.Add(healingOrbCastSounds);
        allSounds.Add(healingOrbHitSounds);
        allSounds.Add(resurrectSounds);
        allSounds.Add(goldSounds);
        allSounds.Add(splashScreenMusicSounds);
        allSounds.Add(musicSounds);
        allSounds.Add(victorySounds);
        allSounds.Add(failSounds);
        allSounds.Add(pickaxeSounds);
        allSounds.Add(rockSmashSounds);
        allSounds.Add(woodBreakingSounds);
        allSounds.Add(swordSlashOpponentSounds);
        allSounds.Add(swordStabOpponentSounds);
        allSounds.Add(throwSounds);
        allSounds.Add(buttonClickSounds);
        allSounds.Add(humanMaleDeathSounds);
        allSounds.Add(humanFemaleDeathSounds);
        allSounds.Add(lichDeathSounds);
        allSounds.Add(skeletonDeathSounds);
        allSounds.Add(zombieDeathSounds);
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

                if (soundArray == musicSounds)
                    _go.GetComponent<AudioSource>().outputAudioMixerGroup = musicAudioMixerGroup;
                else if (SoundIsVoice(soundArray))
                    _go.GetComponent<AudioSource>().outputAudioMixerGroup = voicesAudioMixerGroup;
                else
                    _go.GetComponent<AudioSource>().outputAudioMixerGroup = effectsAudioMixerGroup;
            }
        }

        PlayMusic();
        PlayAmbienceSound();
    }

    public void SetMasterVolume(float volume)
    {
        if (volume <= -60f)
            MuteMasterVolume();
        else
            masterMixer.SetFloat("MasterVolume", volume);
    }

    public void MuteMasterVolume()
    {
        masterMixer.SetFloat("MasterVolume", -80f);
    }

    public void SetMusicVolume(float volume)
    {
        if (volume <= -60f)
            MuteMusicVolume();
        else
            masterMixer.SetFloat("MusicVolume", volume);
    }

    public void MuteMusicVolume()
    {
        masterMixer.SetFloat("MusicVolume", -80f);
    }

    public void SetEffectsVolume(float volume)
    {
        if (volume <= -60f)
            MuteEffectsVolume();
        else
            masterMixer.SetFloat("EffectsVolume", volume);
    }

    public void MuteEffectsVolume()
    {
        masterMixer.SetFloat("EffectsVolume", -80f);
    }

    public void SetVoiceVolume(float volume)
    {
        if (volume <= -40f)
            MuteVoiceVolume();
        else
            masterMixer.SetFloat("VoicesVolume", volume);
    }

    public void MuteVoiceVolume()
    {
        masterMixer.SetFloat("VoicesVolume", -80f);
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

    void PlayMusic()
    {
        int randomIndex = 0;
        if (playedFirstSong)
            randomIndex = Random.Range(0, musicSounds.Length);

        for (int i = 0; i < musicSounds.Length; i++)
        {
            if (musicSounds[randomIndex] == musicSounds[i])
            {
                PlaySound(musicSounds, musicSounds[i].soundName, Vector3.zero);
                Invoke("PlayMusic", musicSounds[i].clip.length);
                return;
            }
        }
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

    public void PlayGoldSound(int goldAmount)
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

    public void PlayRangedHitSound(RangedWeaponType weaponType, bool isAttackingBuilding)
    {
        if (weaponType == RangedWeaponType.Fireball)
            PlayRandomSound(fireballHitSounds);
        else if (weaponType == RangedWeaponType.HealingOrb)
            return; // PlayRandomSound(healingOrbHitSounds);
        else
        {
            if (isAttackingBuilding == false)
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
        else if (weaponType == RangedWeaponType.HealingOrb)
            PlayRandomSound(healingOrbCastSounds);
    }

    public void PlayDeathSound(VoiceType voiceType)
    {
        if (voiceType == VoiceType.HumanMale)
            PlayRandomSound(humanMaleDeathSounds);
        else if (voiceType == VoiceType.HumanFemale)
            PlayRandomSound(humanFemaleDeathSounds);
        else if (voiceType == VoiceType.Skeleton)
            PlayRandomSound(skeletonDeathSounds);
        else if (voiceType == VoiceType.Lich)
            PlayRandomSound(lichDeathSounds);
        else if (voiceType == VoiceType.Zombie)
            PlayRandomSound(zombieDeathSounds);
    }

    bool SoundIsVoice(Sound[] soundArray)
    {
        if (soundArray == humanMaleDeathSounds || soundArray == humanFemaleDeathSounds || soundArray == lichDeathSounds || soundArray == skeletonDeathSounds || soundArray == zombieDeathSounds)
            return true;

        return false;
    }
}
