using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] float defaultVolume = 0.8f;

    [SerializeField] Slider difficultySlider;
    [SerializeField] float defaultDifficulty = 0f;

    MusicPlayer musicPlayer;
    LevelLoader levelLoader;

    void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        levelLoader = FindObjectOfType<LevelLoader>();

        volumeSlider.value = PlayerPrefsController.GetMasterVolume();
        difficultySlider.value = PlayerPrefsController.GetDifficulty();
    }
    
    void Update()
    {
        if (musicPlayer != null)
            musicPlayer.SetVolume(volumeSlider.value);
        else
            Debug.LogWarning("No music player found... did you start from the splash screen?");
    }

    public void SaveAndExit()
    {
        PlayerPrefsController.SetMasterVolume(volumeSlider.value);
        PlayerPrefsController.SetDifficulty(difficultySlider.value);

        levelLoader.CloseOptionsMenu();
    }

    public void SetDefaults()
    {
        volumeSlider.value = defaultVolume;
        difficultySlider.value = defaultDifficulty;
    }
}
