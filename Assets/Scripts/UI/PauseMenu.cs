using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject backdrop;
    [SerializeField] GameObject buttonsParent;
    [SerializeField] GameObject quitConfirmation;
    [SerializeField] GameObject quitToMainMenuConfirmation;

    public bool gamePaused;

    AudioManager audioManager;
    DefenderSpawner defenderSpawner;
    OptionsController optionsController;
    TutorialInfographic tutorialInfographic;
    GameObject textPopupsObjectPoolParent;

    #region Singleton
    public static PauseMenu instance;
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
        defenderSpawner = DefenderSpawner.instance;
        optionsController = OptionsController.instance;
        tutorialInfographic = TutorialInfographic.instance;
        textPopupsObjectPoolParent = GameObject.Find("Text Popups");

        if (backdrop.activeSelf) backdrop.SetActive(false);
        if (buttonsParent.activeSelf) buttonsParent.SetActive(false);
        if (quitConfirmation.activeSelf) quitConfirmation.SetActive(false);
    }

    void Update()
    {
        if (GameControls.gamePlayActions.menuPause.WasPressed && optionsController.optionsMenuOpen == false && tutorialInfographic.isActive == false)
        {
            if (quitConfirmation.activeSelf == false && quitToMainMenuConfirmation.activeSelf == false)
                TogglePauseMenu();
            else if (quitConfirmation.activeSelf)
                ToggleQuitConfirmation();
            else
                ToggleQuitToMainMenuConfirmation();
        }
    }

    public void ToggleQuitConfirmation()
    {
        backdrop.SetActive(!backdrop.activeSelf);
        buttonsParent.SetActive(!buttonsParent.activeSelf);
        quitConfirmation.SetActive(!quitConfirmation.activeSelf);
        PlayButtonClickSound();
    }

    public void ToggleQuitToMainMenuConfirmation()
    {
        backdrop.SetActive(!backdrop.activeSelf);
        buttonsParent.SetActive(!buttonsParent.activeSelf);
        quitToMainMenuConfirmation.SetActive(!quitToMainMenuConfirmation.activeSelf);
        PlayButtonClickSound();
    }

    public void TogglePauseMenu()
    {
        backdrop.SetActive(!backdrop.activeSelf);
        buttonsParent.SetActive(!buttonsParent.activeSelf);

        PlayButtonClickSound();

        if (backdrop.activeSelf)
        {
            defenderSpawner.ClearSelectedSquad();
            defenderSpawner.ClearSelectedStructure();

            textPopupsObjectPoolParent.SetActive(false);
            gamePaused = true;
            Time.timeScale = 0;
        }
        else
        {
            textPopupsObjectPoolParent.SetActive(true);
            gamePaused = false;
            Time.timeScale = 1;
        }
    }

    public void ToggleOptionsMenu()
    {
        optionsController.ToggleOptionsMenu();
    }

    public void ShowHowToPlayInfographic()
    {
        tutorialInfographic.ShowInfographic();
    }

    public void QuitToMainMenu()
    {
        PlayButtonClickSound();
        LevelLoader.instance.LoadMainMenu();
    }

    public void QuitGame()
    {
        PlayButtonClickSound();
        LevelLoader.instance.QuitGame();
    }

    void PlayButtonClickSound()
    {
        audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);
    }
}
