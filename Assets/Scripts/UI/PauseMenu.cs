using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject backdrop;
    [SerializeField] GameObject buttonsParent;
    [SerializeField] GameObject quitConfirmation;
    public bool gamePaused;

    AudioManager audioManager;
    OptionsController optionsController;
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
        optionsController = OptionsController.instance;
        textPopupsObjectPoolParent = GameObject.Find("Text Popups");

        if (backdrop.activeSelf) backdrop.SetActive(false);
        if (buttonsParent.activeSelf) buttonsParent.SetActive(false);
        if (quitConfirmation.activeSelf) quitConfirmation.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && optionsController.optionsMenuOpen == false)
        {
            if (quitConfirmation.activeSelf == false)
                TogglePauseMenu();
            else
                ToggleQuitConfirmation();
        }
    }

    public void ToggleQuitConfirmation()
    {
        backdrop.SetActive(!backdrop.activeSelf);
        buttonsParent.SetActive(!buttonsParent.activeSelf);
        quitConfirmation.SetActive(!quitConfirmation.activeSelf);
        audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);
    }

    public void TogglePauseMenu()
    {
        backdrop.SetActive(!backdrop.activeSelf);
        buttonsParent.SetActive(!buttonsParent.activeSelf);

        audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);

        if (backdrop.activeSelf)
        {
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

    public void QuitGame()
    {
        LevelLoader.instance.QuitGame();
    }
}
