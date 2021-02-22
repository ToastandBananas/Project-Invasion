using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("Main Menu Only")]
    [SerializeField] GameObject newGameConfirmation;
    [SerializeField] int sceneLoadWaitTime = 7;

    [HideInInspector] public int currentLevel = 1;

    int currentSceneIndex;

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
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
            StartCoroutine(WaitToLoadNextScene());
    }

    IEnumerator WaitToLoadNextScene()
    {
        yield return new WaitForSeconds(sceneLoadWaitTime);
        LoadNextScene();
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene("Level " + currentLevel.ToString());
    }

    public void StartNewGame()
    {
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
        newGameConfirmation.SetActive(!newGameConfirmation.activeSelf);
    }
}
