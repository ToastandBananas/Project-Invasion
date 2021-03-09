using UnityEngine;

public class SquadData : MonoBehaviour
{
    string squadDataSavePath = "SquadData.es3";

    [Header("Knight Data")]
    public int knightSquadGoldCost;
    public float knightHealth, knightLeaderHealth;
    public float knightSlashDamage, knightLeaderSlashDamage;

    [Header("Spearmen Data")]
    public int spearmenSquadGoldCost;
    public float spearmenHealth, spearmenLeaderHealth, spearmenAccuracy, spearmenLeaderAccuracy;
    public float spearmenPiercingDamage, spearmenRangedPiercingDamage, spearmenLeaderPiercingDamage, spearmenLeaderRangedPiercingDamage;
    public float spearmenLongThrowTime;
    public bool spearmenLongThrowUnlocked;

    [Header("Archer Data")]
    public int archerSquadGoldCost;
    public float archerHealth, archerLeaderHealth, archerAccuracy, archerLeaderAccuracy;
    public float archerPiercingDamage, archerRangedPiercingDamage, archerLeaderPiercingDamage, archerLeaderRangedPiercingDamage;
    public float archerFireArrowsTime, archerFireArrowsDamageMultiplier, archerRapidFireTime, archerRapidFireSpeedMultipilier;
    public bool archerShouldRetreatWhenEnemyNear, archerFireArrowsUnlocked, archerRapidFireUnlocked;

    // Defaults
    float defaultLongThrowTime = 30f;
    float defaultFireArrowTime = 30f;
    float defaultRapidFireTime = 20f;
    float defaultRapidFireSpeedMultiplier = 2f;

    public void ApplyKnightData(int gold, float health, float leaderHealth, float slashDamage, float leaderSlashDamage)
    {
        knightSquadGoldCost += gold;
        knightHealth += health;
        knightLeaderHealth += leaderHealth;

        knightSlashDamage += slashDamage;
        knightLeaderSlashDamage += leaderSlashDamage;
    }

    public void ApplySpearmenData(int gold, float health, float leaderHealth, float piercingDamage, float leaderPiercingDamage, float rangedPiercingDamage, float leaderRangedPiercingDamage, float accuracy, float leaderAccuracy, float longThrowTime, bool longThrowUnlocked)
    {
        spearmenSquadGoldCost += gold;
        spearmenHealth += health;
        spearmenLeaderHealth += leaderHealth;

        spearmenPiercingDamage += piercingDamage;
        spearmenLeaderPiercingDamage += leaderPiercingDamage;
        spearmenRangedPiercingDamage += rangedPiercingDamage;
        spearmenLeaderRangedPiercingDamage += leaderRangedPiercingDamage;

        spearmenAccuracy += accuracy;
        spearmenLeaderAccuracy += leaderAccuracy;
        spearmenLongThrowTime += longThrowTime;

        if (longThrowUnlocked)
            spearmenLongThrowUnlocked = true;
    }

    public void ApplyArcherData(int gold, float health, float leaderHealth, float piercingDamage, float leaderPiercingDamage, float rangedPiercingDamage, float leaderRangedPiercingDamage, float accuracy, float leaderAccuracy, float fireArrowsTime, float fireArrowsDamageMultiplier, float rapidFireTime, float rapidFireSpeedMultiplier, bool shouldRetreat, bool fireArrowsUnlocked, bool rapidFireUnlocked)
    {
        archerSquadGoldCost += gold;
        archerHealth += health;
        archerLeaderHealth += leaderHealth;

        archerPiercingDamage += piercingDamage;
        archerLeaderPiercingDamage += leaderPiercingDamage;
        archerRangedPiercingDamage += rangedPiercingDamage;
        archerLeaderRangedPiercingDamage += leaderRangedPiercingDamage;

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

    public float GetBluntDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            default:
                return 0f;
        }
    }

    public float GetSlashDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Knights:
                if (isLeader) return knightLeaderSlashDamage;
                else return knightSlashDamage;
            default:
                return 0f;
        }
    }

    public float GetPiercingDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Spearmen:
                if (isLeader) return spearmenLeaderPiercingDamage;
                else return spearmenPiercingDamage;
            case SquadType.Archers:
                if (isLeader) return archerLeaderPiercingDamage;
                else return archerPiercingDamage;
            default:
                return 0f;
        }
    }

    public float GetFireDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            default:
                return 0f;
        }
    }

    public float GetRangedBluntDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            default:
                return 0f;
        }
    }

    public float GetRangedPiercingDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Spearmen:
                if (isLeader) return spearmenLeaderRangedPiercingDamage;
                else return spearmenRangedPiercingDamage;
            case SquadType.Archers:
                if (isLeader) return archerLeaderRangedPiercingDamage;
                else return archerRangedPiercingDamage;
            default:
                return 0f;
        }
    }

    public float GetRangedFireDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
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
        ES3.Save("knightSlashDamage", knightSlashDamage, squadDataSavePath);
        ES3.Save("knightLeaderSlashDamage", knightLeaderSlashDamage, squadDataSavePath);

        // Spearmen Data
        ES3.Save("spearmenSquadGoldCost", spearmenSquadGoldCost, squadDataSavePath);
        ES3.Save("spearmenHealth", spearmenHealth, squadDataSavePath);
        ES3.Save("spearmenLeaderHealth", spearmenLeaderHealth, squadDataSavePath);
        ES3.Save("spearmenPiercingDamage", spearmenPiercingDamage, squadDataSavePath);
        ES3.Save("spearmenLeaderPiercingDamage", spearmenLeaderPiercingDamage, squadDataSavePath);
        ES3.Save("spearmenRangedPiercingDamage", spearmenRangedPiercingDamage, squadDataSavePath);
        ES3.Save("spearmenLeaderRangedPiercingDamage", spearmenLeaderRangedPiercingDamage, squadDataSavePath);
        ES3.Save("spearmenAccuracy", spearmenAccuracy, squadDataSavePath);
        ES3.Save("spearmenLeaderAccuracy", spearmenLeaderAccuracy, squadDataSavePath);
        ES3.Save("spearmenLongThrowTime", spearmenLongThrowTime, squadDataSavePath);
        ES3.Save("spearmenLongThrowUnlocked", spearmenLongThrowUnlocked, squadDataSavePath);

        // Archer Data
        ES3.Save("archerSquadGoldCost", archerSquadGoldCost, squadDataSavePath);
        ES3.Save("archerHealth", archerHealth, squadDataSavePath);
        ES3.Save("archerLeaderHealth", archerLeaderHealth, squadDataSavePath);
        ES3.Save("archerPiercingDamage", archerPiercingDamage, squadDataSavePath);
        ES3.Save("archerLeaderPiercingDamage", archerLeaderPiercingDamage, squadDataSavePath);
        ES3.Save("archerRangedPiercingDamage", archerRangedPiercingDamage, squadDataSavePath);
        ES3.Save("archerLeaderRangedPiercingDamage", archerLeaderRangedPiercingDamage, squadDataSavePath);
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
        knightSlashDamage = ES3.Load("knightSlashDamage", squadDataSavePath, 0f);
        knightLeaderSlashDamage = ES3.Load("knightLeaderSlashDamage", squadDataSavePath, 0f);

        // Spearmen Data
        spearmenSquadGoldCost = ES3.Load("spearmenSquadGoldCost", squadDataSavePath, 0);
        spearmenHealth = ES3.Load("spearmenHealth", squadDataSavePath, 0f);
        spearmenLeaderHealth = ES3.Load("spearmenLeaderHealth", squadDataSavePath, 0f);
        spearmenPiercingDamage = ES3.Load("spearmenPiercingDamage", squadDataSavePath, 0f);
        spearmenLeaderPiercingDamage = ES3.Load("spearmenLeaderPiercingDamage", squadDataSavePath, 0f);
        spearmenRangedPiercingDamage = ES3.Load("spearmenRangedPiercingDamage", squadDataSavePath, 0f);
        spearmenLeaderRangedPiercingDamage = ES3.Load("spearmenLeaderRangedPiercingDamage", squadDataSavePath, 0f);
        spearmenAccuracy = ES3.Load("spearmenAccuracy", squadDataSavePath, 0f);
        spearmenLeaderAccuracy = ES3.Load("spearmenLeaderAccuracy", squadDataSavePath, 0f);
        spearmenLongThrowTime = ES3.Load("spearmenLongThrowTime", squadDataSavePath, defaultLongThrowTime);
        spearmenLongThrowUnlocked = ES3.Load("spearmenLongThrowUnlocked", squadDataSavePath, false);

        // Archer Data
        archerSquadGoldCost = ES3.Load("archerSquadGoldCost", squadDataSavePath, 0);
        archerHealth = ES3.Load("archerHealth", squadDataSavePath, 0f);
        archerLeaderHealth = ES3.Load("archerLeaderHealth", squadDataSavePath, 0f);
        archerPiercingDamage = ES3.Load("archerPiercingDamage", squadDataSavePath, 0f);
        archerLeaderPiercingDamage = ES3.Load("archerLeaderPiercingDamage", squadDataSavePath, 0f);
        archerRangedPiercingDamage = ES3.Load("archerRangedPiercingDamage", squadDataSavePath, 0f);
        archerLeaderRangedPiercingDamage = ES3.Load("archerLeaderRangedPiercingDamage", squadDataSavePath, 0f);
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