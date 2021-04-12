using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Points")]
    public int upgradePoints;
    [SerializeField] Text upgradePointsText;

    [Header("Upgrade Confirmation")]
    [SerializeField] GameObject finishConfirmation;
    [SerializeField] GameObject upgradeConfirmation;

    AudioManager audioManager;
    UpgradeIcon selectedUpgradeIcon;

    #region Singleton
    public static UpgradeManager instance;
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
    }

    public void SetSelectedUpgradeIcon(UpgradeIcon icon)
    {
        selectedUpgradeIcon = icon;
    }

    public void ClearSelectedUpgradeIcon()
    {
        selectedUpgradeIcon = null;
    }

    public void ApplyCurrentlySelectedUpgrade() // Used on the Upgrade Confirmation button (if the player says yes to confirm the upgrade)
    {
        audioManager.PlaySound(audioManager.buttonClickSounds, audioManager.buttonClickSounds[0].soundName, Vector3.zero);
        UseUpgradePoints(selectedUpgradeIcon.upgradePointsCost);
        selectedUpgradeIcon.ApplyUpgrades();
        ToggleUpgradeConfirmationScreen();
    }

    public void ToggleFinishConfirmationScreen()
    {
        finishConfirmation.SetActive(!finishConfirmation.activeSelf);
    }

    public void ToggleUpgradeConfirmationScreen()
    {
        upgradeConfirmation.SetActive(!upgradeConfirmation.activeSelf);

        // If we press the "No" button, play a click sound
        if (upgradeConfirmation.activeSelf == false) 
            audioManager.PlaySound(audioManager.buttonClickSounds, audioManager.buttonClickSounds[0].soundName, Vector3.zero);
    }

    public void UseUpgradePoints(int pointsToUse)
    {
        upgradePoints -= pointsToUse;
        UpdateUpgradePointsText();
    }

    public bool HasEnoughUpgradePoints(int pointsToUse)
    {
        return upgradePoints - pointsToUse >= 0;
    }

    public void UpdateUpgradePointsText()
    {
        upgradePointsText.text = upgradePoints.ToString();
    }

    public void SaveUpgradePoints()
    {
        ES3.Save("upgradePoints", upgradePoints);
    }

    public void LoadUpgradePoints()
    {
        upgradePoints = ES3.Load("upgradePoints", 0);
        UpdateUpgradePointsText();
    }
}
