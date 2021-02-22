using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] float defaultVolume = 0.8f;

    [SerializeField] Slider difficultySlider;
    [SerializeField] float defaultDifficulty = 0f;

    [HideInInspector] public bool optionsMenuOpen;

    MusicPlayer musicPlayer;

    #region Singleton
    public static OptionsController instance;
    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
                Destroy(gameObject);
        }
        else
            instance = this;
    }
    #endregion

    void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();

        LoadSliderValues();

        if (transform.GetChild(0).gameObject.activeSelf)
        {
            optionsMenuOpen = true;
            ToggleOptionsMenu();
        }
    }
    
    void Update()
    {
        if (musicPlayer != null && optionsMenuOpen)
            musicPlayer.SetVolume(volumeSlider.value);
        else if (musicPlayer == null)
            Debug.LogWarning("No music player found... did you start from the splash screen?");
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && optionsMenuOpen)
            SaveAndExit();
    }

    public void SaveAndExit()
    {
        PlayerPrefsController.SetMasterVolume(volumeSlider.value);
        PlayerPrefsController.SetDifficulty(difficultySlider.value);

        ToggleOptionsMenu();
    }

    public void SetDefaults()
    {
        volumeSlider.value = defaultVolume;
        difficultySlider.value = defaultDifficulty;
    }

    public void ToggleOptionsMenu()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!transform.GetChild(i).gameObject.activeSelf);
        }

        optionsMenuOpen = !optionsMenuOpen;
    }

    public void LoadSliderValues()
    {
        volumeSlider.value = PlayerPrefsController.GetMasterVolume();
        difficultySlider.value = PlayerPrefsController.GetDifficulty();
    }
}
