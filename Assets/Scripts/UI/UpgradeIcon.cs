using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Squad Type To Upgrade")]
    [SerializeField] SquadType squadType;

    [Header("Standard Upgrades")]
    [SerializeField] int gold;
    [SerializeField] float health, leaderHealth, meleeDamage, leaderMeleeDamage;

    [Header("Ranged Only Upgrades")]
    [SerializeField] float rangedDamage;
    [SerializeField] float leaderRangedDamage, accuracy, leaderAccuracy;
    [SerializeField] bool shouldRetreat;

    [Header("Spearmen Only Upgrades")]
    [SerializeField] bool longThrowUnlocked;

    [Header("Archer Only Upgrades")]
    [SerializeField] float fireArrowDamageMultiplier;
    [SerializeField] bool fireArrowsUnlocked;

    [Header("Unlocked")]
    public bool upgradeUnlocked;

    SquadData squadData;
    Text upgradeNameText;
    Text upgradeDescriptionText;
    StringBuilder upgradeDescription = new StringBuilder();

    void Start()
    {
        squadData = GameManager.instance.squadData;
        upgradeNameText = GameObject.Find("Upgrade Name Text").GetComponent<Text>();
        upgradeDescriptionText = GameObject.Find("Upgrade Description Text").GetComponent<Text>();

        if (upgradeUnlocked)
            SetIconColor(Color.green);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        BuildDescription();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        ClearDescription();
    }

    public void ApplyUpgrades() // Used in LevelLoader script when going to next level after upgrading in Upgrade Menu
    {
        if (upgradeUnlocked == false)
        {
            switch (squadType)
            {
                case SquadType.Knights:
                    squadData.ApplyKnightData(gold, health, leaderHealth, meleeDamage, leaderMeleeDamage);
                    break;
                case SquadType.Spearmen:
                    squadData.ApplySpearmenData(gold, health, leaderHealth, meleeDamage, leaderMeleeDamage, rangedDamage, leaderRangedDamage, accuracy, leaderAccuracy, longThrowUnlocked);
                    break;
                case SquadType.Archers:
                    squadData.ApplyArcherData(gold, health, leaderHealth, meleeDamage, leaderMeleeDamage, rangedDamage, leaderRangedDamage, accuracy, leaderAccuracy, fireArrowDamageMultiplier, shouldRetreat, fireArrowsUnlocked);
                    break;
                default:
                    break;
            }

            upgradeUnlocked = true;
            SetIconColor(Color.green);
            UpgradeManager.instance.ClearSelectedUpgradeIcon();
            GameManager.instance.SaveCurrentGame();
        }
    }

    public void SelectThisIcon()
    {
        if (upgradeUnlocked == false)
        {
            UpgradeManager.instance.SetSelectedUpgradeIcon(this);
            UpgradeManager.instance.ToggleUpgradeConfirmationScreen();
        }
    }

    void SetIconColor(Color theColor)
    {
        GetComponent<Image>().color = theColor;
    }

    void BuildDescription()
    {
        if (upgradeDescription.Equals("") == false)
            upgradeDescription.Clear();

        // Spearmen Only Upgrades
        if (squadType == SquadType.Spearmen && longThrowUnlocked)
            upgradeDescription.Append("Unlocks the <i>Long Throw</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString() + "</b> will be able to throw their spears the entire length of their lane for the next 30 seconds).\n");

        // Archer Only Upgrades
        if (squadType == SquadType.Archers && shouldRetreat == false)
            upgradeDescription.Append("<b>" + squadType.ToString() + "</b> will now carry melee weapons and will no longer retreat when enemies enter their square.\n");

        if (squadType == SquadType.Archers && fireArrowsUnlocked)
            upgradeDescription.Append("Unlocks the <i>Fire Arrows</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString() + "</b> will shoot fire arrows for the next 30 seconds).\n");

        // General Upgrades
        if (gold > 0)
            upgradeDescription.Append("Gold Cost: <color=green>+" + gold.ToString() + "</color>\n");
        else if (gold < 0)
            upgradeDescription.Append("Gold Cost: <color=red>+" + gold.ToString() + "</color>\n");

        if (health > 0f)
            upgradeDescription.Append("Unit Health: <color=green>+" + health.ToString() + "</color>\n");

        if (leaderHealth > 0f)
            upgradeDescription.Append("Leader Health: <color=green>+" + leaderHealth.ToString() + "</color>\n");

        if (meleeDamage > 0f)
            upgradeDescription.Append("Unit Melee Damage: <color=green>+" + meleeDamage.ToString() + "</color>\n");

        if (leaderMeleeDamage > 0f)
            upgradeDescription.Append("Leader Melee Damage: <color=green>+" + leaderMeleeDamage.ToString() + "</color>\n");

        // Ranged Only Upgrades
        if (rangedDamage > 0f)
            upgradeDescription.Append("Unit Ranged Damage: <color=green>+" + rangedDamage.ToString() + "</color>\n");

        if (leaderRangedDamage > 0f)
            upgradeDescription.Append("Leader Ranged Damage: <color=green>+" + leaderRangedDamage.ToString() + "</color>\n");

        if (accuracy > 0f)
            upgradeDescription.Append("Unit Accuracy: <color=green>+" + accuracy.ToString() + "%</color>\n");

        if (leaderAccuracy > 0f)
            upgradeDescription.Append("Leader Accuracy: <color=green>+" + leaderAccuracy.ToString() + "%</color>\n");

        upgradeNameText.text = gameObject.name;
        upgradeDescriptionText.text = upgradeDescription.ToString();
    }

    void ClearDescription()
    {
        upgradeDescription.Clear();
        upgradeNameText.text = "";
        upgradeDescriptionText.text = "";
    }
}
