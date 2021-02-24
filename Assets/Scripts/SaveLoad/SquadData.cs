using UnityEngine;

public class SquadData : MonoBehaviour
{
    [Header("Knight Data")]
    public int knightSquadGoldCost;
    public float knightHealth, knightLeaderHealth, knightMeleeDamage, knightLeaderMeleeDamage;

    [Header("Spearmen Data")]
    public int spearmenSquadGoldCost;
    public float spearmenHealth, spearmenLeaderHealth, spearmenMeleeDamage, spearmenLeaderMeleeDamage, spearmenRangedDamage, spearmenLeaderRangedDamage, spearmenAccuracy, spearmenLeaderAccuracy;

    [Header("Archer Data")]
    public int archerSquadGoldCost;
    public float archerHealth, archerLeaderHealth, archerMeleeDamage, archerLeaderMeleeDamage, archerRangedDamage, archerLeaderRangedDamage, archerAccuracy, archerLeaderAccuracy, fireArrowsDamageMultiplier;
    public bool archerShouldRetreatWhenEnemyNear, archerFireArrowsUnlocked;

    public void ApplyKnightData(int gold, float health, float leaderHealth, float damage, float leaderDamage)
    {
        knightSquadGoldCost += gold;
        knightHealth += health;
        knightLeaderHealth += leaderHealth;
        knightMeleeDamage += damage;
        knightLeaderMeleeDamage += leaderDamage;
    }

    public void ApplySpearmenData(int gold, float health, float leaderHealth, float meleeDamage, float leaderMeleeDamage, float rangedDamage, float leaderRangedDamage, float accuracy, float leaderAccuracy)
    {
        spearmenSquadGoldCost += gold;
        spearmenHealth += health;
        spearmenLeaderHealth += leaderHealth;
        spearmenMeleeDamage += meleeDamage;
        spearmenLeaderMeleeDamage += leaderMeleeDamage;
        spearmenRangedDamage += rangedDamage;
        spearmenLeaderRangedDamage += leaderRangedDamage;
        spearmenAccuracy += accuracy;
        spearmenLeaderAccuracy += leaderAccuracy;
    }

    public void ApplyArcherData(int gold, float health, float leaderHealth, float meleeDamage, float leaderMeleeDamage, float rangedDamage, float leaderRangedDamage, float accuracy, float leaderAccuracy, float archerFireArrowsDamageMultiplier, bool shouldRetreat, bool fireArrowsUnlocked)
    {
        archerSquadGoldCost += gold;
        archerHealth += health;
        archerLeaderHealth += leaderHealth;
        archerMeleeDamage += meleeDamage;
        archerLeaderMeleeDamage += leaderMeleeDamage;
        archerRangedDamage += rangedDamage;
        archerLeaderRangedDamage += leaderRangedDamage;
        archerAccuracy += accuracy;
        archerLeaderAccuracy += leaderAccuracy;
        fireArrowsDamageMultiplier += archerFireArrowsDamageMultiplier;

        if (shouldRetreat == false)
            archerShouldRetreatWhenEnemyNear = false;

        if (fireArrowsUnlocked)
            archerFireArrowsUnlocked = true;
    }

    public float GetHealthData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Knights:
                if (isLeader) return knightLeaderHealth;
                else return knightHealth;
            case SquadType.Spearmen:
                if (isLeader) return spearmenLeaderHealth;
                else return spearmenHealth;
            case SquadType.Archers:
                if (isLeader) return archerLeaderHealth;
                else return archerHealth;
            default:
                return 0f;
        }
    }

    public float GetMeleeDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Knights:
                if (isLeader) return knightLeaderMeleeDamage;
                else return knightMeleeDamage;
            case SquadType.Spearmen:
                if (isLeader) return spearmenLeaderMeleeDamage;
                else return spearmenMeleeDamage;
            case SquadType.Archers:
                if (isLeader) return archerLeaderMeleeDamage;
                else return archerMeleeDamage;
            default:
                return 0f;
        }
    }

    public float GetRangedDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Spearmen:
                if (isLeader) return spearmenLeaderRangedDamage;
                else return spearmenRangedDamage;
            case SquadType.Archers:
                if (isLeader) return archerLeaderRangedDamage;
                else return archerRangedDamage;
            default:
                return 0f;
        }
    }

    public float GetRangedAccuracyData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Spearmen:
                if (isLeader) return spearmenLeaderAccuracy;
                else return spearmenAccuracy;
            case SquadType.Archers:
                if (isLeader) return archerLeaderAccuracy;
                else return archerAccuracy;
            default:
                return 0f;
        }
    }

    public void SaveSquadData()
    {
        // Knight Data
        ES3.Save("knightSquadGoldCost", knightSquadGoldCost, "SquadData.es3");
        ES3.Save("knightHealth", knightHealth, "SquadData.es3");
        ES3.Save("knightLeaderHealth", knightLeaderHealth, "SquadData.es3");
        ES3.Save("knightMeleeDamage", knightMeleeDamage, "SquadData.es3");
        ES3.Save("knightLeaderMeleeDamage", knightLeaderMeleeDamage, "SquadData.es3");

        // Spearmen Data
        ES3.Save("spearmenSquadGoldCost", spearmenSquadGoldCost, "SquadData.es3");
        ES3.Save("spearmenHealth", spearmenHealth, "SquadData.es3");
        ES3.Save("spearmenLeaderHealth", spearmenLeaderHealth, "SquadData.es3");
        ES3.Save("spearmenMeleeDamage", spearmenMeleeDamage, "SquadData.es3");
        ES3.Save("spearmenLeaderMeleeDamage", spearmenLeaderMeleeDamage, "SquadData.es3");
        ES3.Save("spearmenRangedDamage", spearmenRangedDamage, "SquadData.es3");
        ES3.Save("spearmenLeaderRangedDamage", spearmenLeaderRangedDamage, "SquadData.es3");
        ES3.Save("spearmenAccuracy", spearmenAccuracy, "SquadData.es3");
        ES3.Save("spearmenLeaderAccuracy", spearmenLeaderAccuracy, "SquadData.es3");

        // Archer Data
        ES3.Save("archerSquadGoldCost", archerSquadGoldCost, "SquadData.es3");
        ES3.Save("archerHealth", archerHealth, "SquadData.es3");
        ES3.Save("archerLeaderHealth", archerLeaderHealth, "SquadData.es3");
        ES3.Save("archerMeleeDamage", archerMeleeDamage, "SquadData.es3");
        ES3.Save("archerLeaderMeleeDamage", archerLeaderMeleeDamage, "SquadData.es3");
        ES3.Save("archerRangedDamage", archerRangedDamage, "SquadData.es3");
        ES3.Save("archerLeaderRangedDamage", archerLeaderRangedDamage, "SquadData.es3");
        ES3.Save("archerAccuracy", archerAccuracy, "SquadData.es3");
        ES3.Save("archerLeaderAccuracy", archerLeaderAccuracy, "SquadData.es3");
        ES3.Save("fireArrowDamageMultiplier", fireArrowsDamageMultiplier, "SquadData.es3");
        ES3.Save("archerShouldRetreatWhenEnemyNear", archerShouldRetreatWhenEnemyNear, "SquadData.es3");
        ES3.Save("archerFireArrowsUnlocked", archerFireArrowsUnlocked, "SquadData.es3");
    }

    public void LoadSquadData()
    {
        // Knight Data
        knightSquadGoldCost = ES3.Load("knightSquadGoldCost", "SquadData.es3", 0);
        knightHealth = ES3.Load("knightHealth", "SquadData.es3", 0f);
        knightLeaderHealth = ES3.Load("knightLeaderHealth", "SquadData.es3", 0f);
        knightMeleeDamage = ES3.Load("knightMeleeDamage", "SquadData.es3", 0f);
        knightLeaderMeleeDamage = ES3.Load("knightLeaderMeleeDamage", "SquadData.es3", 0f);

        // Spearmen Data
        spearmenSquadGoldCost = ES3.Load("spearmenSquadGoldCost", "SquadData.es3", 0);
        spearmenHealth = ES3.Load("spearmenHealth", "SquadData.es3", 0f);
        spearmenLeaderHealth = ES3.Load("spearmenLeaderHealth", "SquadData.es3", 0f);
        spearmenMeleeDamage = ES3.Load("spearmenMeleeDamage", "SquadData.es3", 0f);
        spearmenLeaderMeleeDamage = ES3.Load("spearmenLeaderMeleeDamage", "SquadData.es3", 0f);
        spearmenRangedDamage = ES3.Load("spearmenRangedDamage", "SquadData.es3", 0f);
        spearmenLeaderRangedDamage = ES3.Load("spearmenLeaderRangedDamage", "SquadData.es3", 0f);
        spearmenAccuracy = ES3.Load("spearmenAccuracy", "SquadData.es3", 0f);
        spearmenLeaderAccuracy = ES3.Load("spearmenLeaderAccuracy", "SquadData.es3", 0f);

        // Archer Data
        archerSquadGoldCost = ES3.Load("archerSquadGoldCost", "SquadData.es3", 0);
        archerHealth = ES3.Load("archerHealth", "SquadData.es3", 0f);
        archerLeaderHealth = ES3.Load("archerLeaderHealth", "SquadData.es3", 0f);
        archerMeleeDamage = ES3.Load("archerMeleeDamage", "SquadData.es3", 0f);
        archerLeaderMeleeDamage = ES3.Load("archerLeaderMeleeDamage", "SquadData.es3", 0f);
        archerRangedDamage = ES3.Load("archerRangedDamage", "SquadData.es3", 0f);
        archerLeaderRangedDamage = ES3.Load("archerLeaderRangedDamage", "SquadData.es3", 0f);
        archerAccuracy = ES3.Load("archerAccuracy", "SquadData.es3", 0f);
        archerLeaderAccuracy = ES3.Load("archerLeaderAccuracy", "SquadData.es3", 0f);
        fireArrowsDamageMultiplier = ES3.Load("fireArrowDamageMultiplier", "SquadData.es3", 0f);
        archerShouldRetreatWhenEnemyNear = ES3.Load("archerShouldRetreatWhenEnemyNear", "SquadData.es3", true);
        archerFireArrowsUnlocked = ES3.Load("archerFireArrowsUnlocked", "SquadData.es3", false);
    }
}