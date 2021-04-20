using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [Header("Main Menu Only")]
    [SerializeField] GameObject newGameConfirmation;
    [SerializeField] int sceneLoadWaitTime = 7;

    [HideInInspector] public int currentLevel = 1;

    int currentSceneIndex;

    AudioManager audioManager;

    #region Singleton
    public static LevelLoader instance;
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

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
            StartCoroutine(WaitToLoadNextScene());

        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            if (ES3.FileExists("SaveFile.es3") == false)
                GameObject.Find("Continue Button").GetComponent<Button>().interactable = false;
        }
    }

    IEnumerator WaitToLoadNextScene()
    {
        yield return new WaitForSeconds(sceneLoadWaitTime);
        LoadNextScene();
    }

    public void LoadCurrentLevel()
    {
        audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);

        SceneManager.LoadScene("Level " + currentLevel.ToString());
    }

    public void StartNewGame()
    {
        audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);

        GameManager.instance.DeleteAllSaveData();
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadLoseScreen()
    {
        SceneManager.LoadScene("Lose Screen");
    }

    public void LoadUpgradeMenuScene()
    {
        SceneManager.LoadScene("Upgrade Menu");
    }

    public void LoadNextLevel()
    {
        currentLevel++;
        SaveCurrentLevelNumber();
        LoadCurrentLevel();
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);

        Application.Quit();
    }

    public void SaveCurrentLevelNumber()
    {
        ES3.Save("currentLevelNumber", currentLevel);
    }

    public void LoadCurrentLevelNumber()
    {
        currentLevel = ES3.Load("currentLevelNumber", 1);
    }

    public void ToggleNewGameConfirmation()
    {
        if (ES3.FileExists("SaveFile.es3"))
        {
            audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);

            newGameConfirmation.SetActive(!newGameConfirmation.activeSelf);
        }
        else
            StartNewGame();
    }
}
