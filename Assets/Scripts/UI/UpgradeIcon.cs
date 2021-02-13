using UnityEngine;

public class UpgradeIcon : MonoBehaviour
{
    [Header("Squad Type To Upgrade")]
    [SerializeField] SquadType squadType;

    [Header("Standard Upgrades")]
    [SerializeField] int goldUpgradeAmount;
    [SerializeField] float health, leaderHealth, meleeDamage, leaderMeleeDamage;

    [Header("Ranged Only Upgrades")]
    [SerializeField] float rangedDamage;
    [SerializeField] float leaderRangedDamage, accuracy, leaderAccuracy;
    [SerializeField] bool shouldRetreat;

    [Header("Archer Only Upgrades")]
    [SerializeField] bool fireArrowsUnlocked;

    public void ApplyUpgrades()
    {
        switch (squadType)
        {
            case SquadType.Knights:
                SquadData.ApplyKnightData(goldUpgradeAmount, health, leaderHealth, meleeDamage, leaderMeleeDamage);
                break;
            case SquadType.Spearmen:
                SquadData.ApplySpearmenData(goldUpgradeAmount, health, leaderHealth, meleeDamage, leaderMeleeDamage, rangedDamage, leaderRangedDamage, accuracy, leaderAccuracy);
                break;
            case SquadType.Archers:
                SquadData.ApplySpearmenData(goldUpgradeAmount, health, leaderHealth, meleeDamage, leaderMeleeDamage, rangedDamage, leaderRangedDamage, accuracy, leaderAccuracy, shouldRetreat, fireArrowsUnlocked);
                break;
            default:
                break;
        }
    }
}
