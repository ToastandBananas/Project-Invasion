using UnityEngine;

public class ThankYouSceneController : MonoBehaviour
{
    public void GoToMainMenu()
    {
        AudioManager.instance.PlaySound(AudioManager.instance.buttonClickSounds, "MouthClick1", Vector3.zero);
        LevelLoader.instance.LoadMainMenu();
    }

    public void QuitToDesktop()
    {
        LevelLoader.instance.QuitGame();
    }
}
