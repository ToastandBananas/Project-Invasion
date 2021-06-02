using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelLoader : MonoBehaviour
{
    [Header("Main Menu Only")]
    [SerializeField] int sceneLoadWaitTime = 7;
    GameObject newGameConfirmation;

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
            {
                Debug.LogWarning("More than one instance of LevelLoader. Fix me!");
                Destroy(gameObject);
            }
        }
        else
            instance = this;
    }
    #endregion

    void Start()
    {
        audioManager = AudioManager.instance;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // If we're on the Splash Screen, animate the loading text and wait a few seconds to load in the Main Menu
        if (currentSceneIndex == 0)
        {
            StartCoroutine(AnimateLoadingText());
            StartCoroutine(WaitToLoadNextScene());
        }

        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            if (ES3.FileExists("SaveFile.es3") == false)
                GameObject.Find("Continue Button").GetComponent<Button>().interactable = false;

            newGameConfirmation = GameObject.Find("New Game Confirmation");
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

        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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

            for (int i = 0; i < newGameConfirmation.transform.childCount; i++)
            {
                newGameConfirmation.transform.GetChild(i).gameObject.SetActive(!newGameConfirmation.transform.GetChild(i).gameObject.activeSelf);
            }
        }
        else
            StartNewGame();
    }

    IEnumerator AnimateLoadingText()
    {
        Text loadingText = GameObject.Find("Loading Text").GetComponent<Text>();
        float delayTime = 0.2f;

        while (currentSceneIndex == 0)
        {
            yield return new WaitForSeconds(delayTime);
            loadingText.text = ("Loading");
            yield return new WaitForSeconds(delayTime);
            loadingText.text = ("Loading.");
            yield return new WaitForSeconds(delayTime);
            loadingText.text = ("Loading..");
            yield return new WaitForSeconds(delayTime);
            loadingText.text = ("Loading...");
        }
    }
}
