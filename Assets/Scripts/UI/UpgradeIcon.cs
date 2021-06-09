using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Upgrade Points")]
    public int upgradePointsCost = 50;

    [Header("Standard Upgrades")]
    [SerializeField] int gold;
    [SerializeField] int supplies;
    [SerializeField] float health, leaderHealth;

    [Header("Damage Upgrades")]
    [SerializeField] float bluntDamage;
    [SerializeField] float slashDamage, piercingDamage, fireDamage;
    [SerializeField] float leaderBluntDamage, leaderSlashDamage, leaderPiercingDamage, leaderFireDamage;

    [Header("Ranged Only Upgrades")]
    [SerializeField] float accuracy;
    [SerializeField] float leaderAccuracy, rangedBluntDamage, rangedPiercingDamage, rangedFireDamage;
    [SerializeField] float leaderRangedBluntDamage, leaderRangedPiercingDamage, leaderRangedFireDamage;
    [SerializeField] bool shouldRetreat;

    [Header("Archer Only Upgrades")]
    [SerializeField] float fireArrowsTime;
    [SerializeField] float fireArrowsDamageMultiplier, rapidFireTime, rapidFireSpeedMultiplier;
    [SerializeField] bool fireArrowsUnlocked, rapidFireUnlocked;

    [Header("Knight Only Upgrades")]
    [SerializeField] float inspireMultiplier;
    [SerializeField] float inspireTime, thornsDamageMultiplier, thornsTime;
    [SerializeField] bool inspireUnlocked, thornsUnlocked;

    [Header("Laborer Only Upgrades")]
    [SerializeField] float miningSpeedMultiplier;
    [SerializeField] float doubleTimeTime;
    [SerializeField] bool doubleTimeUnlocked;

    [Header("Priest Only Upgrades")]
    [SerializeField] float healAmount;
    [SerializeField] float leaderHealAmount, blessPercentDamageReduction, blessTime;
    [SerializeField] bool blessUnlocked, resurrectUnlocked;

    [Header("Spearmen Only Upgrades")]
    [SerializeField] float longThrowTime;
    [SerializeField] float spearWallTime;
    [SerializeField] bool longThrowUnlocked, spearWallUnlocked;

    [Header("Prerequisites")]
    public UpgradeIcon[] prerequisites;

    [Header("Unlocked Already?")]
    public bool upgradeUnlocked;

    [HideInInspector] public SquadType squadType;
    [HideInInspector] public StructureType structureType;

    AudioManager audioManager;
    SquadData squadData;
    UpgradeManager upgradeManager;
    Text upgradeNameText, upgradeDescriptionText;
    StringBuilder upgradeDescription = new StringBuilder();

    void Start()
    {
        audioManager = AudioManager.instance;
        squadData = GameManager.instance.squadData;
        upgradeManager = UpgradeManager.instance;
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

    public void ApplyUpgrades() // Used in UpgradeManager script when a player confirms that they'd like to purchase an upgrade
    {
        if (upgradeUnlocked == false)
        {
            switch (squadType)
            {
                case SquadType.Archers:
                    squadData.ApplyArcherData(gold, supplies, health, leaderHealth, piercingDamage, leaderPiercingDamage, rangedPiercingDamage, leaderRangedPiercingDamage, accuracy, leaderAccuracy, fireArrowsDamageMultiplier, fireArrowsTime, rapidFireSpeedMultiplier, rapidFireTime, shouldRetreat, fireArrowsUnlocked, rapidFireUnlocked);
                    break;
                case SquadType.Knights:
                    squadData.ApplyKnightData(gold, supplies, health, leaderHealth, slashDamage, leaderSlashDamage, inspireMultiplier, inspireTime, thornsDamageMultiplier, thornsTime, inspireUnlocked, thornsUnlocked);
                    break;
                case SquadType.Laborers:
                    squadData.ApplyLaborerData(gold, supplies, health, miningSpeedMultiplier, doubleTimeTime, doubleTimeUnlocked);
                    break;
                case SquadType.Priests:
                    squadData.ApplyPriestData(gold, supplies, health, leaderHealth, healAmount, leaderHealAmount, blessPercentDamageReduction, blessTime, blessUnlocked, resurrectUnlocked);
                    break;
                case SquadType.Spearmen:
                    squadData.ApplySpearmenData(gold, supplies, health, leaderHealth, piercingDamage, leaderPiercingDamage, rangedPiercingDamage, leaderRangedPiercingDamage, accuracy, leaderAccuracy, longThrowTime, spearWallTime, longThrowUnlocked, spearWallUnlocked);
                    break;
                default:
                    break;
            }

            switch (structureType)
            {
                case StructureType.WoodenStakes:
                    squadData.ApplyWoodenStakesData(gold, supplies, health);
                    break;
                default:
                    break;
            }

            upgradeUnlocked = true;
            SetIconColor(Color.green);
            upgradeManager.ClearSelectedUpgradeIcon();
            GameManager.instance.SaveCurrentGame();
        }
    }

    void BuildDescription() // Builds a tooltip description for the upgrade when the player hovers over the Upgrade Icon
    {
        if (upgradeDescription.Equals("") == false)
            upgradeDescription.Clear();

        // Archer Ability Upgrades
        if (squadType == SquadType.Archers)
        {
            // Stand Strong
            if (shouldRetreat == false)
                upgradeDescription.Append("<b>" + squadType.ToString() + "</b> will now carry melee weapons and will no longer retreat when enemies enter their square.\n");

            // Fire Arrows
            if (fireArrowsUnlocked)
                upgradeDescription.Append("Unlocks the <i>Fire Arrows</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString()
                    + "</b> will shoot fire arrows for the next " + squadData.defaultFireArrowTime.ToString() + " seconds).\n");

            if (fireArrowsDamageMultiplier > 0f)
                upgradeDescription.Append("<i>Fire Arrows</i> Damage: <color=green>+" + fireArrowsDamageMultiplier.ToString() + "%</color>.\n");

            if (fireArrowsTime > 0f)
                upgradeDescription.Append("<i>Fire Arrows</i> Time: <color=green>+" + fireArrowsTime.ToString() + " seconds</color>\n");

            // Rapid Fire
            if (rapidFireUnlocked)
                upgradeDescription.Append("Unlocks the <i>Rapid Fire</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString()
                    + "</b> will shoot twice as fast for the next " + squadData.defaultRapidFireTime.ToString() + " seconds).\n");

            if (rapidFireSpeedMultiplier > 0f)
                upgradeDescription.Append("<i>Rapid Fire</i> Speed: <color=green>+" + (rapidFireSpeedMultiplier * 100f).ToString() + "%</color>.\n");

            if (rapidFireTime > 0f)
                upgradeDescription.Append("<i>Rapid Fire</i> Time: <color=green>+" + rapidFireTime.ToString() + " seconds</color>\n");
        }

        // Knight Ability Upgrades
        else if (squadType == SquadType.Knights)
        {
            // Inspire
            if (inspireUnlocked)
                upgradeDescription.Append("Unlocks the <i>Inspire</i> ability for <b>" + squadType.ToString()
                    + "</b>. (The squad leader will inspire his units and those nearby [in the squares next to his squad in the same lane], increasing their max health and damage by "
                    + (squadData.defaultInspireMultiplier * 100f).ToString() + "% for " + squadData.defaultInspireTime.ToString() + " seconds).\n");

            if (inspireMultiplier > 0f)
                upgradeDescription.Append("<i>Inspire</i> Benefits: <color=green>+" + (inspireMultiplier * 100f).ToString() + "%</color>\n");

            if (inspireTime > 0f)
                upgradeDescription.Append("<i>Inspire</i> Time: <color=green>+" + inspireTime.ToString() + " seconds</color>\n");

            // Thorns
            if (thornsUnlocked)
                upgradeDescription.Append("Unlocks the <i>Thorns</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString()
                    + "</b> will reflect back " + (squadData.defaultThornsDamageMultiplier * 100f).ToString() + "% of the melee damage that they receive for " + squadData.defaultThornsTime.ToString()
                    + " seconds, ignoring all resistances).\n");

            if (thornsDamageMultiplier > 0f)
                upgradeDescription.Append("<i>Thorns</i> Damage: <color=green>+" + (thornsDamageMultiplier * 100f).ToString() + "%</color>\n");

            if (thornsTime > 0f)
                upgradeDescription.Append("<i>Thorns</i> Time: <color=green>+" + thornsTime.ToString() + " seconds</color>\n");
        }

        // Laborer Ability Upgrades
        else if (squadType == SquadType.Laborers)
        {
            if (miningSpeedMultiplier > 0f)
                upgradeDescription.Append("Mining Speed: <color=green>+" + (miningSpeedMultiplier * 100f).ToString() + "%</color>\n");

            // Double Time
            if (doubleTimeUnlocked)
                upgradeDescription.Append("Unlocks the <i>Double Time</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString()
                    + "</b> will mine twice as fast for " + squadData.defaultDoubleTimeTime.ToString() + " seconds).\n");

            if (doubleTimeTime > 0f)
                upgradeDescription.Append("Double Time Time: <color=green>+" + doubleTimeTime.ToString() + " seconds</color>\n");
        }

        // Priest Ability Upgrades
        else if (squadType == SquadType.Priests)
        {
            if (healAmount > 0f)
                upgradeDescription.Append("Healing Power: <color=green>+" + healAmount.ToString() + "</color>\n");

            if (leaderHealAmount > 0f)
                upgradeDescription.Append("Leader Healing Power: <color=green>+" + leaderHealAmount.ToString() + "</color>\n");

            if (blessUnlocked)
                upgradeDescription.Append("Unlocks the <i>Bless</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString()
                    + "</b> will call on the powers above to dramatically reduce incoming damage [by " + (squadData.defaultBlessPercentDamageReduction * 100f).ToString() 
                    + "%] for allies within range for the next " + squadData.defaultBlessTime.ToString() + " seconds).\n");

            if (blessPercentDamageReduction > 0f)
                upgradeDescription.Append("Bless Damage Reduction: <color=green>+" + (blessPercentDamageReduction * 100f).ToString() + "%</color>\n");

            if (blessTime > 0f)
                upgradeDescription.Append("<i>Bless</i> Time: <color=green>+" + blessTime.ToString() + " seconds</color>\n");

            if (resurrectUnlocked)
                upgradeDescription.Append("Unlocks the <i>Resurrect</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString()
                    + "</b> will plead to their god to perform the miracle of raising their nearby allies from the dead. (The resource cost will depend on the unit type and the quantity being resurrected).\n");
        }

        // Spearmen Ability Upgrades
        else if (squadType == SquadType.Spearmen)
        {
            // Long Throw
            if (longThrowUnlocked)
                upgradeDescription.Append("Unlocks the <i>Long Throw</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString() 
                    + "</b> will be able to throw their spears the entire length of their lane for the next " + squadData.defaultLongThrowTime.ToString() + " seconds).\n");

            if (longThrowTime > 0f)
                upgradeDescription.Append("<i>Long Throw</i> Time: <color=green>+" + longThrowTime.ToString() + " seconds</color>\n");

            // Spear Wall
            if (spearWallUnlocked)
                upgradeDescription.Append("Unlocks the <i>Spear Wall</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString()
                    + "</b> will hold the line for the next " + squadData.defaultSpearWallTime.ToString() + " seconds, knocking enemies back with each hit [does not work against large enemies], while taking minimal damage themselves).\n");

            if (spearWallTime > 0f)
                upgradeDescription.Append("<i>Spear Wall</i> Time: <color=green>+" + spearWallTime.ToString() + " seconds</color>\n");
        }

        // General Upgrades
        if (gold > 0)
            upgradeDescription.Append("Gold Cost: <color=red>+" + gold.ToString() + "</color>\n");
        else if (gold < 0)
            upgradeDescription.Append("Gold Cost: <color=green>-" + gold.ToString() + "</color>\n");

        if (supplies > 0)
            upgradeDescription.Append("Supplies Cost: <color=red>+" + supplies.ToString() + "</color>\n");
        else if (supplies < 0)
            upgradeDescription.Append("Supplies Cost: <color=green>-" + supplies.ToString() + "</color>\n");

        if (health > 0f)
        {
            if (squadType != SquadType.Null)
                upgradeDescription.Append("Unit Health: <color=green>+" + health.ToString() + "</color>\n");
            else if (structureType != StructureType.Null)
                upgradeDescription.Append("Structure Health: <color=green>+" + health.ToString() + "</color>\n");
        }

        if (leaderHealth > 0f)
            upgradeDescription.Append("Leader Health: <color=green>+" + leaderHealth.ToString() + "</color>\n");

        // Melee Only Upgrades
        if (bluntDamage > 0f)
            upgradeDescription.Append("Unit Melee Blunt Damage: <color=green>+" + bluntDamage.ToString() + "</color>\n");

        if (leaderBluntDamage > 0f)
            upgradeDescription.Append("Leader Melee Blunt Damage: <color=green>+" + leaderBluntDamage.ToString() + "</color>\n");

        if (slashDamage > 0f)
            upgradeDescription.Append("Unit Melee Slash Damage: <color=green>+" + slashDamage.ToString() + "</color>\n");

        if (leaderSlashDamage > 0f)
            upgradeDescription.Append("Leader Melee Slash Damage: <color=green>+" + leaderSlashDamage.ToString() + "</color>\n");

        if (piercingDamage > 0f)
            upgradeDescription.Append("Unit Melee Piercing Damage: <color=green>+" + piercingDamage.ToString() + "</color>\n");

        if (leaderPiercingDamage > 0f)
            upgradeDescription.Append("Leader Melee Piercing Damage: <color=green>+" + leaderPiercingDamage.ToString() + "</color>\n");

        if (fireDamage > 0f)
            upgradeDescription.Append("Unit Melee Fire Damage: <color=green>+" + fireDamage.ToString() + "</color>\n");

        if (leaderFireDamage > 0f)
            upgradeDescription.Append("Leader Melee Fire Damage: <color=green>+" + leaderFireDamage.ToString() + "</color>\n");

        // Ranged Only Upgrades
        if (rangedBluntDamage > 0f)
            upgradeDescription.Append("Unit Ranged Blunt Damage: <color=green>+" + rangedBluntDamage.ToString() + "</color>\n");

        if (leaderRangedBluntDamage > 0f)
            upgradeDescription.Append("Leader Ranged Blunt Damage: <color=green>+" + leaderRangedBluntDamage.ToString() + "</color>\n");

        if (rangedPiercingDamage > 0f)
            upgradeDescription.Append("Unit Ranged Piercing Damage: <color=green>+" + rangedPiercingDamage.ToString() + "</color>\n");

        if (leaderRangedPiercingDamage > 0f)
            upgradeDescription.Append("Leader Ranged Piercing Damage: <color=green>+" + leaderRangedPiercingDamage.ToString() + "</color>\n");

        if (rangedFireDamage > 0f)
            upgradeDescription.Append("Unit Ranged Fire Damage: <color=green>+" + rangedFireDamage.ToString() + "</color>\n");

        if (leaderRangedFireDamage > 0f)
            upgradeDescription.Append("Leader Ranged Fire Damage: <color=green>+" + leaderRangedFireDamage.ToString() + "</color>\n");

        if (accuracy > 0f)
            upgradeDescription.Append("Unit Accuracy: <color=green>+" + accuracy.ToString() + "%</color>\n");

        if (leaderAccuracy > 0f)
            upgradeDescription.Append("Leader Accuracy: <color=green>+" + leaderAccuracy.ToString() + "%</color>\n");

        // Upgrade Cost
        upgradeDescription.Append("\nUpgrade Cost: <b>" + upgradePointsCost.ToString() + " points</b>");

        upgradeNameText.text = gameObject.name;
        upgradeDescriptionText.text = upgradeDescription.ToString();
    }

    public void SelectThisIcon()
    {
        if (upgradeUnlocked == false && upgradeManager.HasEnoughUpgradePoints(upgradePointsCost))
        {
            bool canUpgrade = false;
            if (prerequisites.Length > 0)
            {
                foreach (UpgradeIcon prerequisite in prerequisites)
                {
                    if (prerequisite.upgradeUnlocked)
                    {
                        canUpgrade = true;
                        break;
                    }
                }
            }
            else // If there's no prerequisites
                canUpgrade = true;

            if (canUpgrade)
            {
                upgradeManager.SetSelectedUpgradeIcon(this);
                upgradeManager.ToggleUpgradeConfirmationScreen();
            }
            else
                audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);
        }
        else
            audioManager.PlaySound(audioManager.buttonClickSounds, "MouthClick1", Vector3.zero);
    }

    void SetIconColor(Color theColor)
    {
        GetComponent<Image>().color = theColor;
    }

    void ClearDescription()
    {
        upgradeDescription.Clear();
        upgradeNameText.text = "";
        upgradeDescriptionText.text = "";
    }
}
