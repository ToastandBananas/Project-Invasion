using UnityEngine;

public class UpgradeIcon : MonoBehaviour
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

    [Header("Archer Only Upgrades")]
    [SerializeField] bool fireArrowsUnlocked;

    [HideInInspector] public bool upgradeUnlocked;

    SquadData squadData;

    void Start()
    {
        squadData = GameManager.instance.squadData;
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
                    squadData.ApplySpearmenData(gold, health, leaderHealth, meleeDamage, leaderMeleeDamage, rangedDamage, leaderRangedDamage, accuracy, leaderAccuracy);
                    break;
                case SquadType.Archers:
                    squadData.ApplySpearmenData(gold, health, leaderHealth, meleeDamage, leaderMeleeDamage, rangedDamage, leaderRangedDamage, accuracy, leaderAccuracy, shouldRetreat, fireArrowsUnlocked);
                    break;
                default:
                    break;
            }

            upgradeUnlocked = true;
            GameManager.instance.SaveCurrentGame();
        }
    }
}
