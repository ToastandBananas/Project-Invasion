﻿using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Squad Type To Upgrade")]
    [SerializeField] SquadType squadType;

    [Header("Standard Upgrades")]
    [SerializeField] int gold;
    [SerializeField] float health, leaderHealth;
    [SerializeField] float bluntDamage, slashDamage, piercingDamage, fireDamage;
    [SerializeField] float leaderBluntDamage, leaderSlashDamage, leaderPiercingDamage, leaderFireDamage;

    [Header("Ranged Only Upgrades")]
    [SerializeField] float accuracy;
    [SerializeField] float leaderAccuracy, rangedBluntDamage, rangedPiercingDamage, rangedFireDamage;
    [SerializeField] float leaderRangedBluntDamage, leaderRangedPiercingDamage, leaderRangedFireDamage;
    [SerializeField] bool shouldRetreat;

    [Header("Knight Only Upgrades")]
    [SerializeField] float thornsDamageMultiplier, thornsTime;
    [SerializeField] bool thornsUnlocked;

    [Header("Spearmen Only Upgrades")]
    [SerializeField] float longThrowTime;
    [SerializeField] bool longThrowUnlocked;

    [Header("Archer Only Upgrades")]
    [SerializeField] float fireArrowsTime, fireArrowsDamageMultiplier, rapidFireTime, rapidFireSpeedMultiplier;
    [SerializeField] bool fireArrowsUnlocked, rapidFireUnlocked;

    [Header("Unlocked Already?")]
    public bool upgradeUnlocked;

    SquadData squadData;
    UpgradeManager upgradeManager;
    Text upgradeNameText;
    Text upgradeDescriptionText;
    StringBuilder upgradeDescription = new StringBuilder();

    void Start()
    {
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

    public void ApplyUpgrades() // Used in LevelLoader script when going to next level after upgrading in Upgrade Menu
    {
        if (upgradeUnlocked == false)
        {
            switch (squadType)
            {
                case SquadType.Knights:
                    squadData.ApplyKnightData(gold, health, leaderHealth, slashDamage, leaderSlashDamage, thornsDamageMultiplier, thornsTime, thornsUnlocked);
                    break;
                case SquadType.Spearmen:
                    squadData.ApplySpearmenData(gold, health, leaderHealth, piercingDamage, leaderPiercingDamage, rangedPiercingDamage, leaderRangedPiercingDamage, accuracy, leaderAccuracy, longThrowTime, longThrowUnlocked);
                    break;
                case SquadType.Archers:
                    squadData.ApplyArcherData(gold, health, leaderHealth, piercingDamage, leaderPiercingDamage, rangedPiercingDamage, leaderRangedPiercingDamage, accuracy, leaderAccuracy, fireArrowsTime, fireArrowsDamageMultiplier, rapidFireTime, rapidFireSpeedMultiplier, shouldRetreat, fireArrowsUnlocked, rapidFireUnlocked);
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

    public void SelectThisIcon()
    {
        if (upgradeUnlocked == false)
        {
            upgradeManager.SetSelectedUpgradeIcon(this);
            upgradeManager.ToggleUpgradeConfirmationScreen();
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

        // Knight Ability Upgrades
        if (squadType == SquadType.Knights)
        {
            if (thornsUnlocked)
                upgradeDescription.Append("Unlocks the <i>Thorns</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString() 
                    + "</b> will reflect back " + (squadData.defaultThornsDamageMultiplier * 100f).ToString() + "% of the melee damage that they receive for " + squadData.defaultThornsTime.ToString() 
                    + " seconds, ignoring all resistances).\n");

            if (thornsDamageMultiplier > 0f)
                upgradeDescription.Append("<i>Thorns</i> Damage Multiplier: <color=green>+" + thornsDamageMultiplier.ToString() + "%</color>\n");

            if (thornsTime > 0f)
                upgradeDescription.Append("<i>Thorns</i> Time: <color=green>+" + thornsTime.ToString() + "seconds</color>\n");
        }

        // Spearmen Ability Upgrades
        if (squadType == SquadType.Spearmen)
        {
            if (longThrowUnlocked)
                upgradeDescription.Append("Unlocks the <i>Long Throw</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString() 
                    + "</b> will be able to throw their spears the entire length of their lane for the next " + squadData.defaultLongThrowTime.ToString() + " seconds).\n");

            if (longThrowTime > 0f)
                upgradeDescription.Append("<i>Long Throw</i> Time: <color=green>+" + longThrowTime.ToString() + " seconds</color>\n");
        }

        // Archer Ability Upgrades
        if (squadType == SquadType.Archers)
        {
            if (shouldRetreat == false)
                upgradeDescription.Append("<b>" + squadType.ToString() + "</b> will now carry melee weapons and will no longer retreat when enemies enter their square.\n");

            if (fireArrowsUnlocked)
                upgradeDescription.Append("Unlocks the <i>Fire Arrows</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString() 
                    + "</b> will shoot fire arrows for the next " + squadData.defaultFireArrowTime.ToString() + " seconds).\n");

            if (rapidFireUnlocked)
                upgradeDescription.Append("Unlocks the <i>Rapid Fire</i> ability for <b>" + squadType.ToString() + "</b>. (<b>" + squadType.ToString() 
                    + "</b> will shoot twice as fast for the next " + squadData.defaultRapidFireTime.ToString() + " seconds).\n");

            if (fireArrowsTime > 0f)
                upgradeDescription.Append("<i>Fire Arrows</i> Time: <color=green>+" + fireArrowsTime.ToString() + " seconds</color>\n");

            if (fireArrowsDamageMultiplier > 0f)
                upgradeDescription.Append("Increases damage for the <i>Fire Arrows</i> ability by a factor of <color=green>" + fireArrowsDamageMultiplier.ToString() + "</color>.\n");

            if (rapidFireTime > 0f)
                upgradeDescription.Append("<i>Rapid Fire</i> Time: <color=green>+" + rapidFireTime.ToString() + " seconds</color>\n");

            if (rapidFireSpeedMultiplier > 0f)
                upgradeDescription.Append("Increases fire speed for the <i>Rapid Fire</i> ability by a factor of <color=green>" + rapidFireSpeedMultiplier.ToString() + "</color>.\n");
        }

        // General Upgrades
        if (gold > 0)
            upgradeDescription.Append("Gold Cost: <color=green>+" + gold.ToString() + "</color>\n");
        else if (gold < 0)
            upgradeDescription.Append("Gold Cost: <color=red>+" + gold.ToString() + "</color>\n");

        if (health > 0f)
            upgradeDescription.Append("Unit Health: <color=green>+" + health.ToString() + "</color>\n");

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
