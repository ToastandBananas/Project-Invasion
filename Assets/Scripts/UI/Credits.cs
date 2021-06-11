using UnityEngine;

public class Credits : MonoBehaviour
{
    [HideInInspector] public bool isActive;

    AudioManager audioManager;

    #region Singleton
    public static Credits instance;
    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Debug.LogWarning("More than one instance of Credits. Fix me!");
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
        if (transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (GameControls.gamePlayActions.menuGoBack.WasPressed && isActive)
            HideCredits();
    }

    public void ShowCredits()
    {
        audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);
        transform.GetChild(0).gameObject.SetActive(true);
        isActive = true;
    }

    public void HideCredits()
    {

        audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);
        transform.GetChild(0).gameObject.SetActive(false);
        isActive = false;
    }
}
