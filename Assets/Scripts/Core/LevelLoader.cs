using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GameObject optionsMenuCanvas;
    [SerializeField] int waitTime = 7;
    int currentSceneIndex;

    void Start()
    {
        if (optionsMenuCanvas != null)
            CloseOptionsMenu();

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
            StartCoroutine(WaitToLoadNextScene());
    }

    IEnumerator WaitToLoadNextScene()
    {
        yield return new WaitForSeconds(waitTime);
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

    public void OpenOptionsMenu()
    {
        optionsMenuCanvas.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        optionsMenuCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
