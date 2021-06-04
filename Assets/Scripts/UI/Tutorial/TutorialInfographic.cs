using UnityEngine;

public class TutorialInfographic : MonoBehaviour
{
    [HideInInspector] public bool isActive;

    AudioManager audioManager;

    #region Singleton
    public static TutorialInfographic instance;
    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Debug.LogWarning("More than one instance of TutorialInfographic. Fix me!");
                Destroy(gameObject);
            }
        }
        else
            instance = this;
    }
    #endregion

    public void Start()
    {
        audioManager = AudioManager.instance;

        if (transform.GetChild(0).gameObject.activeSelf)
            HideInfographic();
    }

    void LateUpdate()
    {
        if (GameControls.gamePlayActions.menuGoBack.WasPressed && isActive)
            HideInfographic();
    }

    public void ShowInfographic()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        isActive = true;

        audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);
    }

    public void HideInfographic()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        isActive = false;

        audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);
    }
}
