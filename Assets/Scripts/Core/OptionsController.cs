using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] Slider volumeSlider;
    [SerializeField] float defaultVolume = 0.8f;

    [Header("Difficulty")]
    [SerializeField] Slider difficultySlider;
    [SerializeField] float defaultDifficulty = 0f;

    [Header("Damage Popups")]
    [SerializeField] Image damagePopupsCheckMark;
    bool defaultDamagePopupsEnabled = true;
    bool damagePopupsEnabled;

    [HideInInspector] public bool optionsMenuOpen;

    AudioManager audioManager;
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
        audioManager = AudioManager.instance;

        LoadSliderValues();
        LoadDamagePopupsBool();

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
        // else if (musicPlayer == null)
            // Debug.LogWarning("No music player found... did you start from the splash screen?");
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
        PlayerPrefsController.SetDamagePopups(damagePopupsEnabled);

        ToggleOptionsMenu();
    }

    public void SetDefaults()
    {
        volumeSlider.value = defaultVolume;
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
        volumeSlider.value = PlayerPrefsController.GetMasterVolume();
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
