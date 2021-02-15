using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
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

    public void ApplyCurrentlySelectedUpgrade()
    {
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
}
