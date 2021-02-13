using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioManager audioManager;
    AudioSource audioSource;

    void Start()
    {
        DontDestroyOnLoad(this);
        audioManager = AudioManager.instance;

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefsController.GetMasterVolume();
    }
    
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
