using UnityEngine;

public class SquadData : MonoBehaviour
{
    string squadDataSavePath = "SquadData.es3";

    [Header("Knight Data")]
    public int knightSquadGoldCost;
    public float knightHealth, knightLeaderHealth, knightMeleeDamage, knightLeaderMeleeDamage;

    [Header("Spearmen Data")]
    public int spearmenSquadGoldCost;
    public float spearmenHealth, spearmenLeaderHealth, spearmenMeleeDamage, spearmenLeaderMeleeDamage, spearmenRangedDamage, spearmenLeaderRangedDamage, spearmenAccuracy, spearmenLeaderAccuracy, spearmenLongThrowTime;
    public bool spearmenLongThrowUnlocked;

    [Header("Archer Data")]
    public int archerSquadGoldCost;
    public float archerHealth, archerLeaderHealth, archerMeleeDamage, archerLeaderMeleeDamage, archerRangedDamage, archerLeaderRangedDamage, archerAccuracy, archerLeaderAccuracy, 
        archerFireArrowsTime, archerFireArrowsDamageMultiplier, archerRapidFireTime, archerRapidFireSpeedMultipilier;
    public bool archerShouldRetreatWhenEnemyNear, archerFireArrowsUnlocked, archerRapidFireUnlocked;

    // Defaults
    float defaultLongThrowTime = 30f;
    float defaultFireArrowTime = 30f;
    float defaultRapidFireTime = 20f;
    float defaultRapidFireSpeedMultiplier = 2f;

    public void ApplyKnightData(int gold, float health, float leaderHealth, float damage, float leaderDamage)
    {
        knightSquadGoldCost += gold;
        knightHealth += health;
        knightLeaderHealth += leaderHealth;
        knightMeleeDamage += damage;
        knightLeaderMeleeDamage += leaderDamage;
    }

    public void ApplySpearmenData(int gold, float health, float leaderHealth, float meleeDamage, float leaderMeleeDamage, float rangedDamage, float leaderRangedDamage, float accuracy, float leaderAccuracy, float longThrowTime, bool longThrowUnlocked)
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
        spearmenLongThrowTime += longThrowTime;

        if (longThrowUnlocked)
            spearmenLongThrowUnlocked = true;
    }

    public void ApplyArcherData(int gold, float health, float leaderHealth, float meleeDamage, float leaderMeleeDamage, float rangedDamage, float leaderRangedDamage, float accuracy, float leaderAccuracy, float fireArrowsTime, float fireArrowsDamageMultiplier, float rapidFireTime, float rapidFireSpeedMultiplier, bool shouldRetreat, bool fireArrowsUnlocked, bool rapidFireUnlocked)
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
        archerFireArrowsTime += fireArrowsTime;
        archerFireArrowsDamageMultiplier += fireArrowsDamageMultiplier;
        archerRapidFireTime += rapidFireTime;
        archerRapidFireSpeedMultipilier += rapidFireSpeedMultiplier;

        if (shouldRetreat == false)
            archerShouldRetreatWhenEnemyNear = false;

        if (fireArrowsUnlocked)
            archerFireArrowsUnlocked = true;

        if (rapidFireUnlocked)
            archerRapidFireUnlocked = true;
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
        ES3.Save("knightSquadGoldCost", knightSquadGoldCost, squadDataSavePath);
        ES3.Save("knightHealth", knightHealth, squadDataSavePath);
        ES3.Save("knightLeaderHealth", knightLeaderHealth, squadDataSavePath);
        ES3.Save("knightMeleeDamage", knightMeleeDamage, squadDataSavePath);
        ES3.Save("knightLeaderMeleeDamage", knightLeaderMeleeDamage, squadDataSavePath);

        // Spearmen Data
        ES3.Save("spearmenSquadGoldCost", spearmenSquadGoldCost, squadDataSavePath);
        ES3.Save("spearmenHealth", spearmenHealth, squadDataSavePath);
        ES3.Save("spearmenLeaderHealth", spearmenLeaderHealth, squadDataSavePath);
        ES3.Save("spearmenMeleeDamage", spearmenMeleeDamage, squadDataSavePath);
        ES3.Save("spearmenLeaderMeleeDamage", spearmenLeaderMeleeDamage, squadDataSavePath);
        ES3.Save("spearmenRangedDamage", spearmenRangedDamage, squadDataSavePath);
        ES3.Save("spearmenLeaderRangedDamage", spearmenLeaderRangedDamage, squadDataSavePath);
        ES3.Save("spearmenAccuracy", spearmenAccuracy, squadDataSavePath);
        ES3.Save("spearmenLeaderAccuracy", spearmenLeaderAccuracy, squadDataSavePath);
        ES3.Save("spearmenLongThrowTime", spearmenLongThrowTime, squadDataSavePath);
        ES3.Save("spearmenLongThrowUnlocked", spearmenLongThrowUnlocked, squadDataSavePath);

        // Archer Data
        ES3.Save("archerSquadGoldCost", archerSquadGoldCost, squadDataSavePath);
        ES3.Save("archerHealth", archerHealth, squadDataSavePath);
        ES3.Save("archerLeaderHealth", archerLeaderHealth, squadDataSavePath);
        ES3.Save("archerMeleeDamage", archerMeleeDamage, squadDataSavePath);
        ES3.Save("archerLeaderMeleeDamage", archerLeaderMeleeDamage, squadDataSavePath);
        ES3.Save("archerRangedDamage", archerRangedDamage, squadDataSavePath);
        ES3.Save("archerLeaderRangedDamage", archerLeaderRangedDamage, squadDataSavePath);
        ES3.Save("archerAccuracy", archerAccuracy, squadDataSavePath);
        ES3.Save("archerLeaderAccuracy", archerLeaderAccuracy, squadDataSavePath);
        ES3.Save("archerFireArrowsTime", archerFireArrowsTime, squadDataSavePath);
        ES3.Save("archerFireArrowDamageMultiplier", archerFireArrowsDamageMultiplier, squadDataSavePath);
        ES3.Save("archerRapidFireTime", archerRapidFireTime, squadDataSavePath);
        ES3.Save("archerRapidFireSpeedMultiplier", archerRapidFireSpeedMultipilier, squadDataSavePath);
        ES3.Save("archerShouldRetreatWhenEnemyNear", archerShouldRetreatWhenEnemyNear, squadDataSavePath);
        ES3.Save("archerFireArrowsUnlocked", archerFireArrowsUnlocked, squadDataSavePath);
        ES3.Save("archerRapidFireUnlocked", archerRapidFireUnlocked, squadDataSavePath);
    }

    public void LoadSquadData()
    {
        // Knight Data
        knightSquadGoldCost = ES3.Load("knightSquadGoldCost", squadDataSavePath, 0);
        knightHealth = ES3.Load("knightHealth", squadDataSavePath, 0f);
        knightLeaderHealth = ES3.Load("knightLeaderHealth", squadDataSavePath, 0f);
        knightMeleeDamage = ES3.Load("knightMeleeDamage", squadDataSavePath, 0f);
        knightLeaderMeleeDamage = ES3.Load("knightLeaderMeleeDamage", squadDataSavePath, 0f);

        // Spearmen Data
        spearmenSquadGoldCost = ES3.Load("spearmenSquadGoldCost", squadDataSavePath, 0);
        spearmenHealth = ES3.Load("spearmenHealth", squadDataSavePath, 0f);
        spearmenLeaderHealth = ES3.Load("spearmenLeaderHealth", squadDataSavePath, 0f);
        spearmenMeleeDamage = ES3.Load("spearmenMeleeDamage", squadDataSavePath, 0f);
        spearmenLeaderMeleeDamage = ES3.Load("spearmenLeaderMeleeDamage", squadDataSavePath, 0f);
        spearmenRangedDamage = ES3.Load("spearmenRangedDamage", squadDataSavePath, 0f);
        spearmenLeaderRangedDamage = ES3.Load("spearmenLeaderRangedDamage", squadDataSavePath, 0f);
        spearmenAccuracy = ES3.Load("spearmenAccuracy", squadDataSavePath, 0f);
        spearmenLeaderAccuracy = ES3.Load("spearmenLeaderAccuracy", squadDataSavePath, 0f);
        spearmenLongThrowTime = ES3.Load("spearmenLongThrowTime", squadDataSavePath, defaultLongThrowTime);
        spearmenLongThrowUnlocked = ES3.Load("spearmenLongThrowUnlocked", squadDataSavePath, false);

        // Archer Data
        archerSquadGoldCost = ES3.Load("archerSquadGoldCost", squadDataSavePath, 0);
        archerHealth = ES3.Load("archerHealth", squadDataSavePath, 0f);
        archerLeaderHealth = ES3.Load("archerLeaderHealth", squadDataSavePath, 0f);
        archerMeleeDamage = ES3.Load("archerMeleeDamage", squadDataSavePath, 0f);
        archerLeaderMeleeDamage = ES3.Load("archerLeaderMeleeDamage", squadDataSavePath, 0f);
        archerRangedDamage = ES3.Load("archerRangedDamage", squadDataSavePath, 0f);
        archerLeaderRangedDamage = ES3.Load("archerLeaderRangedDamage", squadDataSavePath, 0f);
        archerAccuracy = ES3.Load("archerAccuracy", squadDataSavePath, 0f);
        archerLeaderAccuracy = ES3.Load("archerLeaderAccuracy", squadDataSavePath, 0f);
        archerFireArrowsTime = ES3.Load("archerFireArrowsTime", squadDataSavePath, defaultFireArrowTime);
        archerFireArrowsDamageMultiplier = ES3.Load("archerFireArrowDamageMultiplier", squadDataSavePath, 0f);
        archerRapidFireTime = ES3.Load("archerRapidFireTime", squadDataSavePath, defaultRapidFireTime);
        archerRapidFireSpeedMultipilier = ES3.Load("archerRapidFireSpeedMultiplier", squadDataSavePath, defaultRapidFireSpeedMultiplier);
        archerShouldRetreatWhenEnemyNear = ES3.Load("archerShouldRetreatWhenEnemyNear", squadDataSavePath, true);
        archerFireArrowsUnlocked = ES3.Load("archerFireArrowsUnlocked", squadDataSavePath, false);
        archerRapidFireUnlocked = ES3.Load("archerRapidFireUnlocked", squadDataSavePath, false);
    }
}