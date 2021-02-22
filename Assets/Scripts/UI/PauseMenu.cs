using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject backdrop;
    [SerializeField] GameObject buttonsParent;
    [SerializeField] GameObject quitConfirmation;

    public bool gamePaused;

    OptionsController optionsController;

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
        optionsController = OptionsController.instance;

        if (backdrop.activeSelf) backdrop.SetActive(false);
        if (buttonsParent.activeSelf) buttonsParent.SetActive(false);
        if (quitConfirmation.activeSelf) quitConfirmation.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && optionsController.optionsMenuOpen == false)
            TogglePauseMenu();
    }

    public void ToggleQuitConfirmation()
    {
        backdrop.SetActive(!backdrop.activeSelf);
        buttonsParent.SetActive(!buttonsParent.activeSelf);
        quitConfirmation.SetActive(!quitConfirmation.activeSelf);
    }

    public void TogglePauseMenu()
    {
        backdrop.SetActive(!backdrop.activeSelf);
        buttonsParent.SetActive(!buttonsParent.activeSelf);

        if (backdrop.activeSelf)
        {
            gamePaused = true;
            Time.timeScale = 0;
        }
        else
        {
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
        Application.Quit();
    }
}
