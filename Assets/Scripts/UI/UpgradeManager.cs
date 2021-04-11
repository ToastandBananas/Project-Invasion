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
