using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [Header("Master Volume")]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField][Tooltip("Measured in db")][Range(-60f, 0f)] float defaultMasterVolume = 0f;

    [Header("Music Volume")]
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField][Tooltip("Measured in db")][Range(-60f, 0f)] float defaultMusicVolume = -8f;

    [Header("Effects Volume")]
    [SerializeField] Slider effectsVolumeSlider;
    [SerializeField][Tooltip("Measured in db")][Range(-60f, 0f)] float defaultEffectsVolume = 0f;

    [Header("Difficulty")]
    [SerializeField] Slider difficultySlider;
    [SerializeField][Range(0, 2)] int defaultDifficulty = 0;

    [Header("Damage Popups")]
    [SerializeField] Image damagePopupsCheckMark;
    bool defaultDamagePopupsEnabled = true;
    bool damagePopupsEnabled;

    [HideInInspector] public bool optionsMenuOpen;

    AudioManager audioManager;

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
        audioManager = AudioManager.instance;

        LoadSliderValues();
        LoadDamagePopupsBool();

        if (transform.GetChild(0).gameObject.activeSelf)
        {
            optionsMenuOpen = true;
            ToggleOptionsMenu();
        }
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && optionsMenuOpen)
            SaveAndExit();
    }

    public void SaveAndExit()
    {
        PlayerPrefsController.SetMasterVolume(masterVolumeSlider.value);
        PlayerPrefsController.SetMusicVolume(musicVolumeSlider.value);
        PlayerPrefsController.SetEffectsVolume(effectsVolumeSlider.value);
        PlayerPrefsController.SetDifficulty(difficultySlider.value);
        PlayerPrefsController.SetDamagePopups(damagePopupsEnabled);

        ToggleOptionsMenu();
    }

    public void SetDefaults()
    {
        masterVolumeSlider.value = defaultMasterVolume;
        musicVolumeSlider.value = defaultMusicVolume;
        effectsVolumeSlider.value = defaultEffectsVolume;
        difficultySlider.value = defaultDifficulty;
        damagePopupsEnabled = defaultDamagePopupsEnabled;
        damagePopupsCheckMark.gameObject.SetActive(defaultDamagePopupsEnabled);

        audioManager.PlaySound(audioManager.buttonClickSounds, audioManager.buttonClickSounds[0].soundName, Vector3.zero);
    }

    public void ToggleOptionsMenu()
    {
        audioManager.PlaySound(audioManager.buttonClickSounds, audioManager.buttonClickSounds[1].soundName, Vector3.zero);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!transform.GetChild(i).gameObject.activeSelf);
        }

        optionsMenuOpen = !optionsMenuOpen;
    }

    void LoadSliderValues()
    {
        masterVolumeSlider.value = PlayerPrefsController.GetMasterVolume();
        audioManager.SetMasterVolume(masterVolumeSlider.value);

        musicVolumeSlider.value = PlayerPrefsController.GetMusicVolume();
        audioManager.SetMusicVolume(musicVolumeSlider.value);

        effectsVolumeSlider.value = PlayerPrefsController.GetEffectsVolume();
        audioManager.SetEffectsVolume(effectsVolumeSlider.value);

        difficultySlider.value = PlayerPrefsController.GetDifficulty();
    }

    void LoadDamagePopupsBool()
    {
        damagePopupsEnabled = PlayerPrefsController.DamagePopupsEnabled();
        if (damagePopupsEnabled)
            damagePopupsCheckMark.gameObject.SetActive(true);
        else
            damagePopupsCheckMark.gameObject.SetActive(false);
    }

    public void ToggleDamagePopups()
    {
        // Toggle the check mark and the bool
        damagePopupsCheckMark.gameObject.SetActive(!damagePopupsCheckMark.gameObject.activeSelf);
        damagePopupsEnabled = !damagePopupsEnabled;

        audioManager.PlaySound(audioManager.buttonClickSounds, audioManager.buttonClickSounds[0].soundName, Vector3.zero);
    }
}
